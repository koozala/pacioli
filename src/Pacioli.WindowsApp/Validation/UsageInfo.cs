using System.Text.Json;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * The answer of GET /billing/usage: the plan of the key, and its units in the current
     * month (UTC). Each endpoint uses a number of units, for example 3 for a validation.
     */
    public sealed class UsageInfo
    {
        /* The default number of units of one validation, if the answer does not give it. */
        const int DefaultValidateWeight = 3;

        public string Plan { get; init; } = string.Empty;
        public int Quota { get; init; }
        public int Used { get; init; }
        public int Remaining { get; init; }

        /* For example "2026-11". */
        public string Period { get; init; } = string.Empty;

        /* The units of one call of /einvoice/validate. */
        public int ValidateWeight { get; init; } = DefaultValidateWeight;

        public static UsageInfo Parse(string json)
        {
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object || Json.Int(root, "remaining") == null)
            {
                throw new InvalidOperationException("The answer has no field 'remaining'.");
            }

            int weight = Json.Int(root, "default_weight") ?? DefaultValidateWeight;
            if (root.TryGetProperty("unit_weights", out JsonElement weights) && weights.ValueKind == JsonValueKind.Object)
            {
                weight = Json.Int(weights, "/einvoice/validate") ?? weight;
            }

            return new UsageInfo
            {
                Plan = Json.Text(root, "plan") ?? string.Empty,
                Quota = Json.Int(root, "quota") ?? 0,
                Used = Json.Int(root, "used") ?? 0,
                Remaining = Json.Int(root, "remaining") ?? 0,
                Period = Json.Text(root, "period") ?? string.Empty,
                ValidateWeight = weight
            };
        }
    }

    /*
     * The last known quota of the key.
     *
     * The quota belongs to the key, not to one document. Thus all open documents show the
     * same value, and each document hears of a change through the event Changed. The
     * program calls Update on the UI thread only.
     */
    public static class UsageMonitor
    {
        public static UsageInfo? Current { get; private set; }

        /* True after the first usage call, also if that call did not succeed. */
        public static bool HasQueried { get; private set; }

        public static event EventHandler? Changed;

        public static void Update(UsageInfo? usage)
        {
            Current = usage;
            HasQueried = true;
            Changed?.Invoke(null, EventArgs.Empty);
        }
    }
}
