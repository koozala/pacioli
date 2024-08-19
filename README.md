# pacioli
A reader and converter for electronic invoices that adhere to the EN16931 standard.

Download the Windows installer here: [paciolisetup.exe](https://github.com/koozala/pacioli/releases/download/v0.1-alpha/paciolisetup.exe).

<img src="https://github.com/koozala/pacioli/blob/main/doc/Pacioli_Screenshot_1.png" width="450">


# Platform
This program is intended for use on Windows. It's based on .NET 8 and the GUI is implemented with Windows Forms.

# Dependencies
* Pacioli uses the [ZUGFeRD-csharp](https://github.com/stephanstapel/ZUGFeRD-csharp) library for reading electronic invoice files
* [iText](https://itextpdf.com/) is used for generating PDF files
* The preview function is supported by [PDFtoimage](https://github.com/sungaila/PDFtoImage) 

# Status
Pacioli can read XML files representing electronic invoices, show a preview, and save a PDF version of the invoice.

There is no multi-language support yet. The UI and PDF generation is currently all in German.