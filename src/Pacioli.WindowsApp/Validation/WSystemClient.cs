using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Pacioli.WindowsApp.Validation
{
    /* The reason why a call of W System did not succeed. */
    public enum WSystemFailure
    {
        /* The service did not accept the API key (HTTP 401 or 403). */
        InvalidKey,

        /* The quota of the key is used up (HTTP 402 or 429). */
        QuotaExceeded,

        /* The program did not reach the service, or the service did not answer in time. */
        Network,

        /* The service gave an other error, or an answer that the program cannot read. */
        Server
    }

    public sealed class WSystemException : Exception
    {
        public WSystemFailure Failure { get; }

        public HttpStatusCode? StatusCode { get; }

        public WSystemException(WSystemFailure failure, string message, HttpStatusCode? statusCode = null, Exception? inner = null)
            : base(message, inner)
        {
            Failure = failure;
            StatusCode = statusCode;
        }
    }

    /*
     * The client of the W System API (https://api.wsystem.dev).
     *
     * The program uses two endpoints:
     * - POST /einvoice/validate checks an invoice against the XML Schema, EN 16931 and
     *   the KoSIT rules of XRechnung. The body is the file as it is: XML or ZUGFeRD PDF.
     * - GET /billing/usage gives the quota of the key. This call uses no units.
     *
     * Each call sends the key in the header X-API-Key.
     */
    public sealed class WSystemClient
    {
        public static readonly Uri BaseAddress = new Uri("https://api.wsystem.dev/");

        /* One HttpClient for the life of the program, as the documentation of .NET tells. */
        static readonly HttpClient http = CreateHttpClient();

        /* The longest part of an unknown answer that goes into a message. */
        const int MaxBodyInMessage = 500;

        readonly string apiKey;

        public WSystemClient(string apiKey)
        {
            this.apiKey = apiKey;
        }

        static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = BaseAddress,
                Timeout = TimeSpan.FromSeconds(90)
            };

            string version = typeof(WSystemClient).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Pacioli", version));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /*
         * Validates an invoice. The content is the file as the user opened it. A file
         * that starts with "%PDF" goes as PDF, each other file as XML.
         */
        public async Task<ValidationReport> ValidateAsync(byte[] content, string fileName, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "einvoice/validate");
            request.Content = new ByteArrayContent(content);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(IsPdf(content) ? "application/pdf" : "application/xml");

            string json = await SendAsync(request, cancellationToken).ConfigureAwait(false);

            try
            {
                return ValidationReport.Parse(json, fileName, DateTime.Now);
            }
            catch (Exception ex) when (ex is System.Text.Json.JsonException || ex is InvalidOperationException)
            {
                throw new WSystemException(WSystemFailure.Server, Excerpt(json), null, ex);
            }
        }

        /* The quota of the key. */
        public async Task<UsageInfo> GetUsageAsync(CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "billing/usage");
            string json = await SendAsync(request, cancellationToken).ConfigureAwait(false);

            try
            {
                return UsageInfo.Parse(json);
            }
            catch (Exception ex) when (ex is System.Text.Json.JsonException || ex is InvalidOperationException)
            {
                throw new WSystemException(WSystemFailure.Server, Excerpt(json), null, ex);
            }
        }

        /*
         * The quota of the key, or null if the service does not give it. The quota is
         * information only, thus an error must not stop the user.
         */
        public async Task<UsageInfo?> TryGetUsageAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetUsageAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (WSystemException)
            {
                return null;
            }
        }

        async Task<string> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("X-API-Key", apiKey);

            HttpResponseMessage response;
            try
            {
                response = await http.SendAsync(request, cancellationToken).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                throw new WSystemException(WSystemFailure.Network, ex.Message, null, ex);
            }
            catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
            {
                /* HttpClient gives TaskCanceledException when the time is over. */
                throw new WSystemException(WSystemFailure.Network, ex.Message, null, ex);
            }

            using (response)
            {
                string body = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return body;
                }

                WSystemFailure failure = response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => WSystemFailure.InvalidKey,
                    HttpStatusCode.PaymentRequired or HttpStatusCode.TooManyRequests => WSystemFailure.QuotaExceeded,
                    _ => WSystemFailure.Server
                };

                string message = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
                if (!string.IsNullOrWhiteSpace(body))
                {
                    message += Environment.NewLine + Excerpt(body);
                }

                throw new WSystemException(failure, message, response.StatusCode);
            }
        }

        static bool IsPdf(byte[] content)
        {
            byte[] signature = Encoding.ASCII.GetBytes("%PDF");
            return content.AsSpan().StartsWith(signature);
        }

        static string Excerpt(string text)
        {
            return text.Length <= MaxBodyInMessage ? text : text.Substring(0, MaxBodyInMessage) + " …";
        }
    }
}
