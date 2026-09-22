; MeowPlayer - Inno Setup installer

#define MyAppName "MeowPlayer"
#define MyAppPublisher "ahenyao"
#define MyAppURL "https://000000404.xyz"
#define MyAppExeName "MeowPlayer.Desktop.exe"


#ifndef MyAppVersion
  #define MyAppVersion "undefined"
#endif

#ifndef AppArch
  #define AppArch "win-x64"
#endif


#if AppArch == "win-x64"
  #define SourceDir "..\build\win-x64"
#else
  #define SourceDir "..\build\win-x86"
#endif


[Setup]

AppId={{2C4458EF-9360-43FA-BC0B-52B2623E4A6B}

AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}

AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}

DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}

UninstallDisplayIcon={app}\{#MyAppExeName}

CloseApplications=yes
RestartApplications=no

#if AppArch == "win-x64"
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
#endif

LicenseFile=..\LICENSE

PrivilegesRequired=admin
PrivilegesRequiredOverridesAllowed=dialog

OutputDir=dist
OutputBaseFilename={#MyAppName}-{#MyAppVersion}-{#AppArch}-Setup

SolidCompression=yes
WizardStyle=classic dynamic


[Languages]

Name: "english"; MessagesFile: "compiler:Default.isl"


[Tasks]

Name: "desktopicon"; \
    Description: "{cm:CreateDesktopIcon}"; \
    GroupDescription: "{cm:AdditionalIcons}"; \
    Flags: unchecked


[Files]

Source: "{#SourceDir}\*"; \
    DestDir: "{app}"; \
    Flags: ignoreversion recursesubdirs createallsubdirs


[Icons]

Name: "{group}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"

Name: "{group}\{cm:ProgramOnTheWeb,{#MyAppName}}"; \
    Filename: "{#MyAppURL}"

Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; \
    Filename: "{uninstallexe}"

Name: "{autodesktop}\{#MyAppName}"; \
    Filename: "{app}\{#MyAppExeName}"; \
    Tasks: desktopicon


[Run]

Filename: "{app}\{#MyAppExeName}"; \
    Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; \
    Flags: nowait postinstall skipifsilent