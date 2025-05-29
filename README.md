# Pacioli

_For the English version, see [here](README_en.md)._

Ein Windows-Tool zum Lesen und Konvertieren von elektronischen Rechnungen. 

Die neueste Version lässt sich auf der Webseite [www.epacioli.de](https://www.epacioli.de/) herunterladen.

Die E-Rechnung ist im Wesentlichen eine XML-Datei, die sich nur schwer direkt lesen lässt. Mit Pacioli lassen sich aus diesen XML-Dateien lesbare PDF-Dateien erzeugen. Diese stellen **keinen Ersatz** für die XML-Dateien dar! 
Massgeblich für die Verarbeitung und Archivierung von Rechnungen ist die XML-Datei. Eine zugehörige PDF-Datei dient lediglich zur leichteren Überprüfung der Angaben in der XML-Datei.

Neben den "einfachen" XML-Dateien verarbeitet Pacioli auch die sogenannten ZUGFeRD-Dateien. Das sind PDFs, die die XML-Datei als Anhang mitbringen. Mit Pacioli lässt sich eine unabhängige PDF-Datei erstellen, so dass man die 
Angaben in der ZUGFeRD-PDF überprüfen kann.

Feedback, Ideen, Anfragen an [pacioli@silent-pulse.com](mailto:pacioli@silent-pulse.com).

## Benutzung

Die neueste Version von Pacioli kann hier heruntergeladen werden: [Aktuelle Pacioli-Version](https://www.epacioli.de/FileDownload/Download?filename=paciolisetup.exe). Pacioli ist kostenlos und kann frei verwendet werden.

Nach der Installation lässt sich das Programm über das Windows-Startmenü starten.

<img src="https://github.com/koozala/pacioli/blob/main/doc/Pacioli_Screenshot_1.png" width="450">


## Werkzeug für die Kommandozeile

Ein separates Tool findet sich im Programmverzeichnis: `pacioli-cmd.exe`. Dieses lässt sich nutzen, um mehrere E-Rechnungen automatisch zu verarbeiten.

 
#### Beispiel: Lies eine XML-Datei ein und erzeuge ein PDF-Dokument dazu

`pacioli-cmd.exe -in erech.xml -pdfname erech.pdf`


#### Beispiel: XML-Anhang einer Zugferd-Datei extrahieren und abspeichern

Liest eine Zugferd-Datei mit dem Namen "zugferdfile.pdf" ein und speichert die darin integrierte XML-Datei unter dem Namen "erech.xml" in den Ordner "c:\tmp".

`pacioli-cmd.exe -in zugferdfile.pdf -output-folder c:\tmp -xmlname erech.xml`

#### Alle Optionen anzeigen

```
pacioli-cmd.exe -?
Usage:
pacioli-cmd.exe -in <file> [-output-folder <folder>] [-genpdf [-pdfname <file>]] [-xmlname <file>]
 -?                        Show usage
 -in <file>                Read input from file, could be a XML or a PDF file (Zugferd input)
 -genpdf                   Generate PDF rendering; if no pdfname is given, the filename will be generated
 -output-folder <folder>   Where to put output files
 -pdfname <file>           Name of the generated PDF file
 -xmlname <file>           If given, the XML file will be copied/extracted
```

