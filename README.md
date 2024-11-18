# pacioli
A reader and converter for electronic invoices that adhere to the EN16931 standard.

_Lesen und Anzeigen von E-Rechnungen sowie Konvertierung nach PDF. Die neueste Version findet sich [hier](https://github.com/koozala/pacioli/releases/latest)._

Pacioli can read XML and ZUGFeRD files representing electronic invoices, show a preview, and save a PDF version of the invoice.

You can download the latest release for Windows [here](https://github.com/koozala/pacioli/releases/latest).

<img src="https://github.com/koozala/pacioli/blob/main/doc/Pacioli_Screenshot_1.png" width="450">

## Command Line Utility

The package includes a command line `pacioli-cmd.exe` that can be used to process E-Rechnung/Zugferd files in batch jobs and similar scenarios.

#### Beispiel: Lies eine XML-Datei ein und erzeuge ein PDF-Dokument dazu

`pacioli-cmd.exe -in erech.xml -pdfname erech.pdf`


#### Beispiel: XML-Anhang einer Zugferd-Datei extrahieren und abspeichern

Liest eine Zugferd-Datei mit dem Namen "zugferdfile.pdf" ein und speichert die darin integrierte XML-Datei unter dem Namen "erech.xml" in den Ordner "c:\tmp".

`pacioli-cmd.exe -in zugferdfile.pdf -output-folder c:\tmp -xmlname erech.xml`




# Platform
This program is intended for use on Windows. It's based on .NET 8 and the GUI is implemented with Windows Forms.

# Dependencies
* Pacioli uses the [ZUGFeRD-csharp](https://github.com/stephanstapel/ZUGFeRD-csharp) library for reading electronic invoice files
* [iText](https://itextpdf.com/) is used for generating PDF files
* The preview function is supported by [PDFtoimage](https://github.com/sungaila/PDFtoImage) 

# Multi-language support

The goal is to support many languages for document creation, and possibly the UI. The foundation for multi-language support has been added. Currently German and English are supported for document creation.


