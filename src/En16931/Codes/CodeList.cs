using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace En16931.Codes;

/// <summary>
/// The language of the name of a code.
/// </summary>
public enum CodeLanguage
{
    /// <summary>The name of the official code list of the specification.</summary>
    English,

    /// <summary>
    /// The German name. The specification gives no German code list, thus the German table
    /// is kept by hand and it does not hold every code. A code without a German name falls
    /// back to the English name.
    /// </summary>
    German
}

/// <summary>
/// A lookup table that maps the value of a code to a readable name.
/// </summary>
/// <remarks>
/// <para>
/// The English names come from the official code list workbook of the specification. The
/// German names are kept by hand and they cover the lists and the codes that a German
/// invoice really shows; see <see cref="GermanCount"/>. Get a list from
/// <see cref="CodeLists"/>.
/// </para>
/// <para>
/// A list reads its entries on first use, thus a list that the application never touches
/// costs no time and no memory. The lookup is case sensitive, because the codes of the
/// specification are case sensitive.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// CodeLists.DocumentType.GetName("380");                        // "Commercial invoice"
/// CodeLists.DocumentType.GetName("380", CodeLanguage.German);   // "Handelsrechnung"
/// CodeLists.DocumentType.Describe("380", CodeLanguage.German);  // "380 (Handelsrechnung)"
/// </code>
/// </example>
public sealed class CodeList
{
    private readonly Lazy<FrozenDictionary<string, string>> english;
    private readonly Lazy<FrozenDictionary<string, string>> german;
    private readonly Lazy<FrozenDictionary<string, string>> extra;

    /*
     * extraData and extraGermanData hold the codes that documents of the practice use and
     * that the workbook of the specification does not give. See CodeListDataExtra.
     */
    internal CodeList(
        string id,
        string name,
        string data,
        string? germanData = null,
        string? extraData = null,
        string? extraGermanData = null)
    {
        Id = id;
        Name = name;
        english = new Lazy<FrozenDictionary<string, string>>(() => Parse(data, extraData));
        german = new Lazy<FrozenDictionary<string, string>>(
            () => germanData is null && extraGermanData is null
                ? FrozenDictionary<string, string>.Empty
                : Parse(germanData ?? string.Empty, extraGermanData));
        extra = new Lazy<FrozenDictionary<string, string>>(
            () => extraData is null ? FrozenDictionary<string, string>.Empty : Parse(extraData, null));
    }

    /// <summary>
    /// Identifier of the code list, for example <c>1001</c> for the document type codes or
    /// <c>ISO 3166-1</c> for the country codes.
    /// </summary>
    public string Id { get; }

    /// <summary>Readable name of the code list, for example <c>Document type code</c>.</summary>
    public string Name { get; }

    /// <summary>All entries of the list in English, from the code to the name.</summary>
    public IReadOnlyDictionary<string, string> Entries => english.Value;

    /// <summary>
    /// The entries that have a German name. This table is smaller than
    /// <see cref="Entries"/>, and for some lists it is empty.
    /// </summary>
    public IReadOnlyDictionary<string, string> GermanEntries => german.Value;

    /// <summary>The number of entries of the list.</summary>
    public int Count => english.Value.Count;

    /// <summary>The number of entries that have a German name.</summary>
    public int GermanCount => german.Value.Count;

    /// <summary>
    /// The entries that do not come from the workbook of the specification. Documents of
    /// the practice hold these codes. See <c>CodeListDataExtra</c>.
    /// </summary>
    public IReadOnlyDictionary<string, string> ExtraEntries => extra.Value;

    /// <summary>All codes of the list.</summary>
    public IEnumerable<string> Codes => english.Value.Keys;

    /// <summary>
    /// Gives the English name of a code, or <see langword="null"/> if the list does not
    /// hold the code. A <see langword="null"/> code also gives <see langword="null"/>.
    /// </summary>
    public string? GetName(string? code) =>
        code is not null && english.Value.TryGetValue(code.Trim(), out var name) ? name : null;

    /// <summary>
    /// Gives the name of a code in the given language. If the code has no German name, the
    /// English name comes back instead.
    /// </summary>
    public string? GetName(string? code, CodeLanguage language) =>
        language == CodeLanguage.German ? GetGermanName(code) ?? GetName(code) : GetName(code);

    /// <summary>
    /// Gives the German name of a code, or <see langword="null"/> if the German table does
    /// not hold the code. Use <see cref="GetName(string?, CodeLanguage)"/> to fall back to
    /// the English name.
    /// </summary>
    public string? GetGermanName(string? code) =>
        code is not null && german.Value.TryGetValue(code.Trim(), out var name) ? name : null;

    /// <summary>Tries to get the name of a code.</summary>
    public bool TryGetName(string? code, [NotNullWhen(true)] out string? name)
    {
        name = GetName(code);
        return name is not null;
    }

    /// <summary>Tries to get the name of a code in the given language.</summary>
    public bool TryGetName(string? code, CodeLanguage language, [NotNullWhen(true)] out string? name)
    {
        name = GetName(code, language);
        return name is not null;
    }

    /// <summary>Tells if the list holds the code.</summary>
    public bool Contains(string? code) => GetName(code) is not null;

    /// <summary>
    /// Gives the code together with its name, for example <c>380 (Commercial invoice)</c>.
    /// An unknown code gives only the code itself, thus the result is always usable for a
    /// display or for a log.
    /// </summary>
    public string Describe(string? code) => Describe(code, CodeLanguage.English);

    /// <summary>
    /// Gives the code together with its name in the given language, for example
    /// <c>380 (Handelsrechnung)</c>. An unknown code gives only the code itself.
    /// </summary>
    public string Describe(string? code, CodeLanguage language)
    {
        if (string.IsNullOrWhiteSpace(code)) return string.Empty;

        var trimmed = code.Trim();
        return GetName(trimmed, language) is { } name ? $"{trimmed} ({name})" : trimmed;
    }

    /// <summary>The same as <see cref="GetName(string?)"/>.</summary>
    public string? this[string? code] => GetName(code);

    /// <summary>The name of the list, its identifier, and the number of its entries.</summary>
    public override string ToString() =>
        GermanCount == 0
            ? $"{Name} ({Id}), {Count} entries"
            : $"{Name} ({Id}), {Count} entries, {GermanCount} of them also German";

    /// <summary>Reads the entries out of the data: one line per entry, <c>code|name</c>.</summary>
    private static FrozenDictionary<string, string> Parse(string data, string? extraData)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        Collect(data, result, keepFirst: false);

        if (extraData is not null)
        {
            /*
             * An extra entry fills a hole only. If the official list already holds the
             * code, the official name stays: the name of the specification has priority.
             */
            Collect(extraData, result, keepFirst: true);
        }

        return result.ToFrozenDictionary(StringComparer.Ordinal);

        static void Collect(string text, Dictionary<string, string> into, bool keepFirst)
        {
            foreach (var line in text.AsSpan().EnumerateLines())
            {
                if (line.IsEmpty) continue;

                var divider = line.IndexOf('|');
                if (divider < 0) continue;

                var code = line[..divider].ToString();
                var name = line[(divider + 1)..].ToString();

                if (keepFirst)
                {
                    into.TryAdd(code, name);
                }
                else
                {
                    into[code] = name;
                }
            }
        }
    }
}
