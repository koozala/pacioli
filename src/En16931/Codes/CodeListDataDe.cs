namespace En16931.Codes;

/// <summary>
/// German names of the codes. The format is the same as <see cref="CodeListData"/>:
/// one line per entry, the code and the name divided by a vertical bar.
/// </summary>
/// <remarks>
/// <para>
/// The specification gives its code lists only in English and in French, thus this table
/// is kept by hand. It holds the lists and the codes that a German invoice really shows.
/// A code without an entry here falls back to the English name, thus a lookup always gives
/// a result.
/// </para>
/// <para>
/// A code in this file must also be in <see cref="CodeListData"/>. Add a new entry only
/// together with the code of the official list.
/// </para>
/// </remarks>
internal static class CodeListDataDe
{
    internal const string DocumentType =
        """
        71|Zahlungsanforderung
        80|Belastungsanzeige für Waren oder Dienstleistungen
        81|Gutschrift für Waren oder Dienstleistungen
        82|Rechnung für verbrauchsabhängige Leistungen
        83|Gutschrift für finanzielle Anpassungen
        84|Belastungsanzeige für finanzielle Anpassungen
        102|Steuermitteilung
        130|Rechnungsdatenblatt
        202|Direkte Zahlungsbewertung
        203|Vorläufige Zahlungsbewertung
        204|Zahlungsbewertung
        211|Zwischenzahlungsantrag
        218|Schlusszahlungsanforderung nach Fertigstellung
        219|Zahlungsanforderung für fertiggestellte Einheiten
        261|Gutschrift im Gutschriftverfahren
        262|Sammelgutschrift für Waren und Dienstleistungen
        295|Rechnung über eine Preisänderung
        296|Gutschrift über eine Preisänderung
        308|Delkredere-Gutschrift
        325|Proformarechnung
        326|Teilrechnung
        331|Handelsrechnung mit Packliste
        380|Handelsrechnung
        381|Gutschrift
        382|Provisionsabrechnung
        383|Belastungsanzeige
        384|Rechnungskorrektur
        385|Sammelrechnung
        386|Vorauszahlungsrechnung
        387|Mietrechnung
        388|Steuerrechnung
        389|Rechnung im Gutschriftverfahren (Selbstfakturierung)
        390|Delkredere-Rechnung
        393|Factoring-Rechnung
        394|Leasingrechnung
        395|Konsignationsrechnung
        396|Factoring-Gutschrift
        420|Gutschrift für eine OCR-Zahlung
        456|Belastungsanzeige
        457|Stornierung einer Belastung
        458|Stornierung einer Gutschrift
        471|Korrekturrechnung im Gutschriftverfahren
        472|Korrekturrechnung mit Factoring
        473|Korrekturrechnung im Gutschriftverfahren mit Factoring
        500|Vorauszahlungsrechnung im Gutschriftverfahren
        501|Rechnung im Gutschriftverfahren mit Factoring
        502|Gutschrift im Gutschriftverfahren mit Factoring
        503|Gutschrift zu einer Vorauszahlung
        527|Belastungsanzeige im Gutschriftverfahren
        532|Gutschrift des Spediteurs
        553|Abweichungsbericht zur Speditionsrechnung
        575|Rechnung des Versicherers
        623|Speditionsrechnung
        633|Belege über Hafengebühren
        751|Rechnungsinformation für Buchungszwecke
        780|Frachtrechnung
        817|Schadenanzeige
        870|Konsulatsfaktura
        875|Abschlagsrechnung (Bau)
        876|Teilschlussrechnung (Bau)
        877|Schlussrechnung (Bau)
        935|Zollrechnung
        50|Preis- oder Verkaufskatalog (Antwort)
        916|Zusätzliche Nachweisdokumente
        """;

    internal const string TaxCategory =
        """
        S|Regelsatz
        Z|Nullsatz (Waren mit Steuersatz 0 %)
        E|Steuerbefreit
        AE|Umkehrung der Steuerschuldnerschaft (Reverse Charge)
        K|Steuerfreie innergemeinschaftliche Lieferung
        G|Steuerfreie Ausfuhrlieferung
        O|Nicht steuerbare Leistung
        L|Allgemeine indirekte Steuer der Kanarischen Inseln (IGIC)
        M|Steuer auf Produktion, Dienstleistungen und Einfuhr in Ceuta und Melilla (IPSI)
        """;

    internal const string TaxType =
        """
        VAT|Umsatzsteuer
        AAA|Mineralölsteuer
        AAD|Tabaksteuer
        AAE|Energieabgabe
        AAF|Kaffeesteuer
        """;

    internal const string PaymentMeans =
        """
        1|Zahlungsart nicht festgelegt
        2|Gutschrift über eine automatisierte Clearingstelle
        3|Lastschrift über eine automatisierte Clearingstelle
        9|Nationales oder regionales Clearing
        10|Bar
        20|Scheck
        21|Bankscheck
        22|Bestätigter Bankscheck
        23|Bankscheck (von einer Bank ausgestellt)
        24|Wechsel, zur Annahme vorgelegt
        25|Bestätigter Scheck
        26|Ortsscheck
        30|Überweisung
        31|Lastschriftübertragung
        42|Zahlung auf ein Bankkonto
        44|Angenommener Wechsel
        45|Referenzierte Homebanking-Überweisung
        46|Interbanken-Lastschrift
        47|Homebanking-Lastschrift
        48|Bankkarte
        49|Lastschrift
        50|Zahlung per Postgiro
        52|Eilige Handelszahlung
        53|Eilige Zahlung der Staatskasse
        54|Kreditkarte
        55|Debitkarte
        56|Bankgiro
        57|Dauerauftrag
        58|SEPA-Überweisung
        59|SEPA-Lastschrift
        60|Solawechsel
        68|Online-Bezahldienst
        70|Vom Gläubiger auf den Schuldner gezogener Wechsel
        91|Nicht übertragbarer Bankscheck
        92|Nicht übertragbarer Ortsscheck
        93|Referenz-Giro
        94|Eilgiro
        95|Giro im freien Format
        96|Die gewünschte Zahlungsart wurde nicht verwendet
        97|Verrechnung zwischen den Partnern
        ZZZ|Gegenseitig vereinbart
        """;

    internal const string NoteSubject =
        """
        AAA|Warenbeschreibung
        AAB|Zahlungsbedingung
        AAC|Zusätzliche Angaben zu Gefahrgut
        AAI|Allgemeine Information
        AAJ|Zusätzliche Verkaufs- oder Einkaufsbedingungen
        AAK|Preisbedingungen
        AAR|Lieferbedingungen
        ABL|Behördliche Information
        ABN|Buchhalterische Information
        ACB|Zusätzliche Information
        ACC|Abtretungsklausel (Factoring)
        ADU|Hinweis
        DEL|Lieferinformation
        PMD|Zahlungsdetail oder Überweisungsinformation
        PMT|Zahlungsinformation
        REG|Regulatorische Information
        SUR|Anmerkungen des Lieferanten
        TXD|Steuerangaben
        """;

    internal const string AllowanceReason =
        """
        41|Bonus für eine vorzeitige Fertigstellung
        42|Sonstiger Bonus
        60|Verbraucherrabatt des Herstellers
        62|Wegen Militärstatus
        63|Wegen Arbeitsunfall
        64|Sondervereinbarung
        65|Rabatt wegen eines Produktionsfehlers
        66|Rabatt für eine neue Verkaufsstelle
        67|Musterrabatt
        68|Auslaufrabatt
        70|Incoterm-Rabatt
        71|Nachlass bei Erreichen einer Verkaufsschwelle
        88|Materialzuschlag oder Materialabzug
        95|Rabatt
        100|Sonderrabatt
        102|Langfristig festgelegt
        103|Vorübergehend
        104|Standard
        105|Jahresumsatz
        """;

    internal const string ChargeReason =
        """
        AA|Werbung
        AAA|Telekommunikation
        ABK|Sonstiges
        ABL|Zusätzliche Verpackung
        ADR|Sonstige Dienstleistungen
        CG|Reinigung
        FC|Frachtkosten
        FI|Finanzierung
        PC|Verpackung
        SH|Besondere Behandlung
        ZZZ|Gegenseitig vereinbart
        """;

    internal const string LineStatus =
        """
        1|Hinzugefügt
        2|Gelöscht
        3|Geändert
        4|Keine Aktion
        5|Ohne Änderung angenommen
        6|Mit Änderung angenommen
        7|Nicht angenommen
        8|Nur Terminplan
        9|Änderungen
        10|Nicht gefunden
        11|Nicht geändert
        12|Positionsnummern geändert
        13|Der Käufer hat den Betrag abgezogen
        14|Der Käufer erhebt einen Einwand gegen die Rechnung
        15|Rückbelastung durch den Verkäufer
        16|Der Verkäufer stellt eine Gutschrift aus
        17|Bedingungen auf neue Bedingungen geändert
        18|Das Ergebnis der Verhandlungen abwarten
        19|Der Verkäufer weist den Einwand zurück
        20|Ausgleich
        21|Keine Lieferung
        22|Abruflieferung
        """;

    internal const string LineStatusReason =
        """
        DETAIL|Reguläre Position (Standardfall)
        GROUP|Zwischensumme oder Gruppe
        INFORMATION|Nur zur Information
        """;

    internal const string DateFormat =
        """
        102|JJJJMMTT
        205|JJJJMMTTHHMM mit Zeitzone
        610|JJJJMM
        616|JJJJWW (Kalenderwoche)
        """;

    internal const string TimeReference =
        """
        5|Rechnungsdatum
        29|Lieferdatum
        72|Zahlungsdatum
        """;

    internal const string Incoterms =
        """
        1|Lieferung durch den Lieferanten veranlasst
        2|Lieferung durch den Logistikdienstleister veranlasst
        CFR|Kosten und Fracht (benannter Bestimmungshafen)
        CIF|Kosten, Versicherung und Fracht (benannter Bestimmungshafen)
        CIP|Frachtfrei versichert (benannter Bestimmungsort)
        CPT|Frachtfrei (benannter Bestimmungsort)
        DAP|Geliefert benannter Ort
        DDP|Geliefert verzollt (benannter Bestimmungsort)
        DPU|Geliefert benannter Ort entladen
        EXW|Ab Werk (benannter Ort der Lieferung)
        FAS|Frei Längsseite Schiff (benannter Verschiffungshafen)
        FCA|Frei Frachtführer (benannter Ort der Lieferung)
        FOB|Frei an Bord (benannter Verschiffungshafen)
        """;

    internal const string TransportMode =
        """
        0|Transportart nicht angegeben
        1|Seetransport
        2|Schienentransport
        3|Straßentransport
        4|Lufttransport
        5|Post
        6|Multimodaler Transport
        7|Feste Transportinstallation
        8|Binnenschifftransport
        9|Transportart nicht anwendbar
        """;

    internal const string MimeCode =
        """
        application/pdf|PDF-Dokument
        image/png|PNG-Bild
        image/jpeg|JPEG-Bild
        text/csv|CSV-Datei
        application/vnd.openxmlformats-officedocument.spreadsheetml.sheet|Excel-Arbeitsmappe (XLSX)
        application/vnd.oasis.opendocument.spreadsheet|OpenDocument-Tabelle (ODS)
        """;

    /// <summary>The units that an invoice really uses. See UN/ECE Recommendation 20 and 21.</summary>
    internal const string UnitOfMeasure =
        """
        H87|Stück
        C62|Einheit
        EA|Stück
        NAR|Anzahl der Artikel
        SET|Satz
        PR|Paar
        DZN|Dutzend
        CEN|Hundert
        TP|Zehnerpack
        LS|Pauschale
        P1|Prozent
        E48|Dienstleistungseinheit
        ZZ|Gegenseitig vereinbart
        DAY|Tag
        WEE|Woche
        MON|Monat
        ANN|Jahr
        HUR|Stunde
        MIN|Minute
        SEC|Sekunde
        KGM|Kilogramm
        GRM|Gramm
        TNE|Tonne
        LTR|Liter
        MLT|Milliliter
        MTR|Meter
        CMT|Zentimeter
        MMT|Millimeter
        KMT|Kilometer
        SMI|Meile
        MTK|Quadratmeter
        MTQ|Kubikmeter
        KWH|Kilowattstunde
        MWH|Megawattstunde
        BLL|Barrel (US)
        XBG|Beutel
        XBC|Flaschenkasten
        XBX|Box
        XCH|Kiste
        XCR|Steige
        XCS|Kasten
        XCT|Karton
        XPA|Päckchen
        XPK|Packstück
        XPP|Stück
        XPX|Palette
        XRO|Rolle
        XTU|Tube
        """;

    /// <summary>The reference types that an invoice really uses.</summary>
    internal const string ReferenceType =
        """
        ON|Bestellnummer des Käufers
        VN|Auftragsnummer des Verkäufers
        BD|Angebotsnummer
        CT|Vertragsnummer
        DQ|Lieferscheinnummer
        IV|Rechnungsnummer
        PL|Preislistennummer
        AAJ|Lieferauftragsnummer
        AAU|Versandanzeigennummer
        ALO|Wareneingangsanzeigennummer
        ACD|Zusätzliche Referenznummer
        """;

    /// <summary>The classification schemes that an invoice really uses.</summary>
    internal const string ItemTypeIdentification =
        """
        HS|Harmonisiertes System (Zolltarifnummer)
        STQ|eCl@ss
        TST|UNSPSC
        SRV|GS1 Global Trade Item Number (GTIN)
        MF|Artikelnummer des Herstellers
        MP|Produkt- oder Dienstleistungsnummer
        GN|Nationaler Warengruppenschlüssel
        BD|Nationaler Arzneimittelcode (Format 5-3-2)
        ZZZ|Gegenseitig vereinbart
        """;

    /// <summary>The schemes of an electronic address that an invoice really uses.</summary>
    internal const string ElectronicAddressScheme =
        """
        0060|D-U-N-S-Nummer
        0088|GLN (EAN-Lokationsnummer)
        0204|Leitweg-ID
        9930|Deutsche Umsatzsteuer-Identifikationsnummer
        EM|E-Mail-Adresse
        0002|SIRENE (Frankreich)
        0009|SIRET (Frankreich)
        """;

    /// <summary>The identifier schemes that an invoice really uses.</summary>
    internal const string IdentifierScheme =
        """
        0060|D-U-N-S-Nummer
        0088|GLN (EAN-Lokationsnummer)
        0160|GTIN (Global Trade Item Number)
        0002|SIRENE (Frankreich)
        0009|SIRET (Frankreich)
        """;

    /// <summary>The currencies that a German invoice usually uses.</summary>
    internal const string Currency =
        """
        EUR|Euro
        USD|US-Dollar
        CHF|Schweizer Franken
        GBP|Pfund Sterling
        JPY|Yen
        PLN|Zloty
        CZK|Tschechische Krone
        DKK|Dänische Krone
        SEK|Schwedische Krone
        NOK|Norwegische Krone
        HUF|Forint
        RON|Rumänischer Leu
        RSD|Serbischer Dinar
        ISK|Isländische Krone
        TRY|Türkische Lira
        UAH|Hrywnja
        RUB|Russischer Rubel
        CNY|Renminbi Yuan
        HKD|Hongkong-Dollar
        SGD|Singapur-Dollar
        KRW|Won
        INR|Indische Rupie
        THB|Baht
        ILS|Neuer Israelischer Schekel
        AED|VAE-Dirham
        CAD|Kanadischer Dollar
        USN|US-Dollar (nächster Tag)
        AUD|Australischer Dollar
        NZD|Neuseeland-Dollar
        ZAR|Rand
        BRL|Brasilianischer Real
        MXN|Mexikanischer Peso
        MAD|Marokkanischer Dirham
        TND|Tunesischer Dinar
        """;

    /// <summary>Europe and the countries that a German invoice usually names.</summary>
    internal const string Country =
        """
        AD|Andorra
        AE|Vereinigte Arabische Emirate
        AL|Albanien
        AM|Armenien
        AR|Argentinien
        AT|Österreich
        AU|Australien
        AX|Åland
        AZ|Aserbaidschan
        BA|Bosnien und Herzegowina
        BE|Belgien
        BG|Bulgarien
        BR|Brasilien
        BY|Belarus
        CA|Kanada
        CH|Schweiz
        CL|Chile
        CN|China
        CO|Kolumbien
        CY|Zypern
        CZ|Tschechien
        DE|Deutschland
        DK|Dänemark
        DZ|Algerien
        EE|Estland
        EG|Ägypten
        ES|Spanien
        FI|Finnland
        FO|Färöer
        FR|Frankreich
        GB|Vereinigtes Königreich
        GE|Georgien
        GG|Guernsey
        GI|Gibraltar
        GL|Grönland
        GR|Griechenland
        HK|Hongkong
        HR|Kroatien
        HU|Ungarn
        ID|Indonesien
        IE|Irland
        IL|Israel
        IM|Isle of Man
        IN|Indien
        IQ|Irak
        IR|Iran
        IS|Island
        IT|Italien
        JE|Jersey
        JP|Japan
        KR|Republik Korea (Südkorea)
        KZ|Kasachstan
        LI|Liechtenstein
        LT|Litauen
        LU|Luxemburg
        LV|Lettland
        MA|Marokko
        MC|Monaco
        MD|Republik Moldau
        ME|Montenegro
        MK|Nordmazedonien
        MT|Malta
        MX|Mexiko
        MY|Malaysia
        NG|Nigeria
        NL|Niederlande
        NO|Norwegen
        NZ|Neuseeland
        PE|Peru
        PH|Philippinen
        PK|Pakistan
        PL|Polen
        PT|Portugal
        QA|Katar
        RO|Rumänien
        RS|Serbien
        RU|Russische Föderation
        SA|Saudi-Arabien
        SE|Schweden
        SG|Singapur
        SI|Slowenien
        SK|Slowakei
        SM|San Marino
        TH|Thailand
        TN|Tunesien
        TR|Türkei
        TW|Taiwan
        UA|Ukraine
        US|Vereinigte Staaten
        UY|Uruguay
        VA|Vatikanstadt
        VN|Vietnam
        ZA|Südafrika
        """;
}
