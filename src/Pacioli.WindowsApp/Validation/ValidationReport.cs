using System.Text.Json;

namespace Pacioli.WindowsApp.Validation
{
    /*
     * The answer of POST /einvoice/validate.
     *
     * The OpenAPI document of W System does not give a schema for the answer. Thus the
     * parser reads each field by itself, and a field that is not there keeps its default.
     * A new field of the service does no harm.
     */
    public sealed class ValidationReport
    {
        public bool Valid { get; init; }
        public int ErrorCount { get; init; }
        public int WarningCount { get; init; }
        public int? FormatErrorCount { get; init; }
        public int? BusinessRuleErrorCount { get; init; }

        /* For example "xml" or "pdf". */
        public string? Container { get; init; }

        /* For example "ubl" or "cii". */
        public string? Syntax { get; init; }

        /* For example "XRechnung". */
        public string? Profile { get; init; }

        public string? CustomizationId { get; init; }
        public string? Disclaimer { get; init; }

        /* The rule sets, and if the service checked them. */
        public IReadOnlyDictionary<string, bool> Checked { get; init; } = new Dictionary<string, bool>();

        public IReadOnlyDictionary<string, string> RuleVersions { get; init; } = new Dictionary<string, string>();

        public IReadOnlyList<string> Notes { get; init; } = Array.Empty<string>();

        public IReadOnlyList<ValidationFinding> Errors { get; init; } = Array.Empty<ValidationFinding>();

        public IReadOnlyList<ValidationFinding> Warnings { get; init; } = Array.Empty<ValidationFinding>();

        /* The German view of the result, or null if it does not apply. */
        public GermanVatStatus? Germany { get; init; }

        /* The name of the file that the user checked. */
        public string FileName { get; init; } = string.Empty;

        /* The local time of the check. */
        public DateTime CheckedAt { get; init; }

        /* The answer as the service gave it. */
        public string RawJson { get; init; } = string.Empty;

        public static ValidationReport Parse(string json, string fileName, DateTime checkedAt)
        {
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("valid", out _))
            {
                throw new InvalidOperationException("The answer has no field 'valid'.");
            }

            return new ValidationReport
            {
                Valid = Json.Bool(root, "valid") ?? false,
                ErrorCount = Json.Int(root, "error_count") ?? 0,
                WarningCount = Json.Int(root, "warning_count") ?? 0,
                FormatErrorCount = Json.Int(root, "format_error_count"),
                BusinessRuleErrorCount = Json.Int(root, "business_rule_error_count"),
                Container = Json.Text(root, "container"),
                Syntax = Json.Text(root, "syntax"),
                Profile = Json.Text(root, "profile"),
                CustomizationId = Json.Text(root, "customization_id"),
                Disclaimer = Json.Text(root, "disclaimer"),
                Checked = ReadChecked(root),
                RuleVersions = ReadTexts(root, "rule_versions"),
                Notes = Json.TextList(root, "notes"),
                Errors = ReadFindings(root, "errors"),
                Warnings = ReadFindings(root, "warnings"),
                Germany = GermanVatStatus.Read(root),
                FileName = fileName,
                CheckedAt = checkedAt,
                RawJson = json
            };
        }

        static IReadOnlyDictionary<string, bool> ReadChecked(JsonElement root)
        {
            var result = new Dictionary<string, bool>();
            if (root.TryGetProperty("checked", out JsonElement element) && element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.True || property.Value.ValueKind == JsonValueKind.False)
                    {
                        result[property.Name] = property.Value.GetBoolean();
                    }
                }
            }
            return result;
        }

        static IReadOnlyDictionary<string, string> ReadTexts(JsonElement root, string name)
        {
            var result = new Dictionary<string, string>();
            if (root.TryGetProperty(name, out JsonElement element) && element.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    result[property.Name] = Json.AsText(property.Value);
                }
            }
            return result;
        }

        static IReadOnlyList<ValidationFinding> ReadFindings(JsonElement root, string name)
        {
            var result = new List<ValidationFinding>();
            if (root.TryGetProperty(name, out JsonElement element) && element.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in element.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Object)
                    {
                        result.Add(ValidationFinding.Read(item));
                    }
                }
            }
            return result;
        }
    }

    /* One error or one warning of the validation. */
    public sealed class ValidationFinding
    {
        /* The official ID of the rule, for example "BR-CO-10", or "XSD". */
        public string RuleId { get; init; } = string.Empty;

        /* "error" or "warning". */
        public string? Severity { get; init; }

        /* For example "EN16931" or "XRechnung". */
        public string? Ruleset { get; init; }

        public string Message { get; init; } = string.Empty;

        /* The XPath of the element. */
        public string? Location { get; init; }

        /* For example "BT-27" or "BG-8". */
        public IReadOnlyList<string> BusinessTerms { get; init; } = Array.Empty<string>();

        /* A hint how to correct the error, if the service knows one. */
        public string? Fix { get; init; }

        /* "format" (XML Schema) or "business_rule" (Schematron). */
        public string? ErrorClass { get; init; }

        /* The provisions of the German VAT law that the error touches. */
        public IReadOnlyList<GermanVatItem> GermanVatItems { get; init; } = Array.Empty<GermanVatItem>();

        internal static ValidationFinding Read(JsonElement item)
        {
            var vatItems = new List<GermanVatItem>();
            if (item.TryGetProperty("de_vat_items", out JsonElement element) && element.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement vat in element.EnumerateArray())
                {
                    if (vat.ValueKind == JsonValueKind.Object)
                    {
                        vatItems.Add(new GermanVatItem
                        {
                            BusinessTerm = Json.Text(vat, "business_term"),
                            Provision = Json.Text(vat, "provision"),
                            Item = Json.Text(vat, "item"),
                            Conditional = Json.Bool(vat, "conditional") ?? false
                        });
                    }
                }
            }

            return new ValidationFinding
            {
                RuleId = Json.Text(item, "rule_id") ?? string.Empty,
                Severity = Json.Text(item, "severity"),
                Ruleset = Json.Text(item, "ruleset"),
                Message = Json.Text(item, "message") ?? string.Empty,
                Location = Json.Text(item, "location"),
                BusinessTerms = Json.TextList(item, "business_terms"),
                Fix = Json.Text(item, "fix"),
                ErrorClass = Json.Text(item, "error_class"),
                GermanVatItems = vatItems
            };
        }
    }

    public sealed class GermanVatItem
    {
        public string? BusinessTerm { get; init; }

        /* For example "§ 14 Abs. 4 Satz 1 Nr. 1 UStG". */
        public string? Provision { get; init; }

        public string? Item { get; init; }

        public bool Conditional { get; init; }
    }

    /*
     * How the BMF letter reads the result, for an invoice with a German seller or buyer.
     * For example, a business-rule error without a format error is in principle still an
     * e-invoice for VAT.
     */
    public sealed class GermanVatStatus
    {
        /* For example "format_error". */
        public string? Status { get; init; }

        public string? Explanation { get; init; }

        internal static GermanVatStatus? Read(JsonElement root)
        {
            if (!root.TryGetProperty("germany", out JsonElement element) || element.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            if (Json.Bool(element, "applies") == false)
            {
                return null;
            }

            return new GermanVatStatus
            {
                Status = Json.Text(element, "de_vat_status"),
                Explanation = Json.Text(element, "explanation")
            };
        }
    }

    /* Reads one field of a JSON object. A field of an other type gives null. */
    internal static class Json
    {
        public static string? Text(JsonElement element, string name)
        {
            if (element.TryGetProperty(name, out JsonElement value) && value.ValueKind != JsonValueKind.Null)
            {
                string text = AsText(value);
                return string.IsNullOrWhiteSpace(text) ? null : text;
            }
            return null;
        }

        public static int? Int(JsonElement element, string name)
        {
            if (element.TryGetProperty(name, out JsonElement value) && value.ValueKind == JsonValueKind.Number
                && value.TryGetInt32(out int number))
            {
                return number;
            }
            return null;
        }

        public static bool? Bool(JsonElement element, string name)
        {
            if (element.TryGetProperty(name, out JsonElement value))
            {
                if (value.ValueKind == JsonValueKind.True) return true;
                if (value.ValueKind == JsonValueKind.False) return false;
            }
            return null;
        }

        public static IReadOnlyList<string> TextList(JsonElement element, string name)
        {
            var result = new List<string>();
            if (element.TryGetProperty(name, out JsonElement value) && value.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement item in value.EnumerateArray())
                {
                    string text = AsText(item);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        result.Add(text);
                    }
                }
            }
            return result;
        }

        /* A text as it is. Each other value as JSON text. */
        public static string AsText(JsonElement value)
        {
            return value.ValueKind == JsonValueKind.String ? value.GetString() ?? string.Empty : value.GetRawText();
        }
    }
}
