; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Pacioli"
#define MyAppVersion "0.10"
#define MyAppPublisher "EDV-Beratung Vogt GmbH"
#define MyAppURL "https://www.edv-vogt.de"
#define MyAppExeName "Pacioli.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B3DB4CD5-6719-4885-84A1-4FD2BA37E918}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\wrk\repos\Rechnungsdruck_AS400_PDF\Pacioli - OSS ERechnung PDF\github\pacioli\LICENSE
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=S:\wrk\Projekte\RechPdf\Pacioli.Installer
OutputBaseFilename=paciolisetup
SetupIconFile=C:\wrk\repos\Rechnungsdruck_AS400_PDF\Graphics\Pacioli_128.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "dutch"; MessagesFile: "compiler:Languages\Dutch.isl"
Name: "french"; MessagesFile: "compiler:Languages\French.isl"
Name: "german"; MessagesFile: "compiler:Languages\German.isl"
Name: "hungarian"; MessagesFile: "compiler:Languages\Hungarian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "S:\wrk\Projekte\RechPdf\Pacioli.NET.Selfcontained\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "S:\wrk\Projekte\RechPdf\Pacioli.NET.Selfcontained\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "S:\wrk\Projekte\RechPdf\Pacioli.Commandline\pacioli-cmd.exe"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "S:\wrk\Projekte\RechPdf\Pacioli.Commandline\pacioli-cmd.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
