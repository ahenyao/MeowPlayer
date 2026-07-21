
# MeowPlayer

Music player for desktop and mobile platforms with an Aero-inspired theme.
Built with .NET and AvaloniaUI. Focuses on clean interface without ads.

![C#](https://img.shields.io/badge/C%23-purple?style=plastic)
![AvaloniaUI](https://img.shields.io/badge/AvaloniaUI-007DF9?style=plastic&logo=avaloniaui&logoColor=white)
[![PolyForm Noncommercial](https://img.shields.io/badge/License-PolyForm_Noncommercial_1.0.0-15a05d?style=plastic)](LICENSE)

![Linux](https://img.shields.io/badge/Linux-FCC624?style=plastic&logo=linux&logoColor=black)
![macOS](https://img.shields.io/badge/macOS-lightgray?style=plastic&logo=apple&logoColor=black)
![Windows](https://img.shields.io/badge/⊞_Windows-0078D4?style=plastic)
![iOS](https://img.shields.io/badge/iOS-121212?style=plastic&logo=apple&logoColor=white)
![Android](https://img.shields.io/badge/Android-3DDC84?style=plastic&logo=android&logoColor=white)


## Screenshots

Coming soon.


## Features

 * Aero-inspired theme
 * No ads or tracking
 * Local audio playback
 * Playback speed and pitch adjustment
 


## Platform support

| Platform | Status |
|----------|--------|
| Linux | Fully supported |
| Windows | Supported |
| macOS | Supported |
| Android | Fully supported |
| iOS | Experimental |

- **Fully supported** platforms receive regular testing for every build and have priority for bug fixes.
- **Supported** platforms are maintained and receive bug fixes, but less thorough testing.
- **Experimental** platforms receive limited testing. They should work, but platform-specific issues may occur.


## Download

Prebuilt binaries for Linux, macOS, Windows and Android are available on the [Releases](https://github.com/ahenyao/MeowPlayer/releases) page.

**iOS builds must be compiled manually from source and signed with an Apple Developer account. See [iOS-Guide.md](iOS-Guide.md)**


## Building

### Prerequisites
Make sure you have the [.NET 10 SDK](https://dotnet.microsoft.com/download) installed on your system.

For Android and iOS, you need to install the corresponding workloads.

```bash
dotnet workload install android
```
or
```bash
dotnet workload install ios
```

### Building

1. Clone the repository

```bash
git clone --recursive https://github.com/ahenyao/MeowPlayer.git
cd MeowPlayer
```

2. Compile for your platform

> [!TIP]  
> By default, all builds end up in the `build` directory

**Desktop (Linux, Windows, macOS):**

```bash
dotnet publish MeowPlayer.Desktop/MeowPlayer.Desktop.csproj -c Release
```
> [!NOTE]  
> Cross-compilation for different OS and architectures is possible by adding .NET `-r` (Runtime Identifier) parameter.

Valid target RIDs include:
* `linux-x64` | `linux-arm` | `linux-arm64`
* `win-x64` | `win-x86`
* `osx-x64` | `osx-arm64`

For example to compile for **ARM64 Linux** regardless of your current host OS, run:
```bash
dotnet publish MeowPlayer.Desktop/MeowPlayer.Desktop.csproj -c Release -r linux-arm64
```

---

**Android:**

```bash
dotnet publish MeowPlayer.Android/MeowPlayer.Android.csproj -c Release
```

> [!NOTE]  
> By default, the APK targets `android-x64` and `android-arm64`. To compile for a different architecture, you need to add `-r` (Runtime Identifier) parameter.
Valid target RIDs include:
* `android-x86` | `android-x64` 
* `android-arm` | `android-arm64`

For example to compile for **x86 Android**, run:
```bash
dotnet publish MeowPlayer.Android/MeowPlayer.Android.csproj -c Release -r android-x86
```
<!-- If you want to get APK file which works on more than one architecture, you need to add `-p:RuntimeIdentifiers=""` parameter. Inside the quotes you have to type RIDs separated by a semicolon (`;`)

For example to compile for  **ARM32 Android** and **ARM64 Android**, run:
```bash
dotnet publish MeowPlayer.Android/MeowPlayer.Android.csproj -c Release -p:RuntimeIdentifiers="android-arm;android-arm64"
``` -->

---

**iOS:**

Please refer to [iOS-Guide.md](iOS-Guide.md)

---


## Support

If you encounter any bugs or have feature requests, please [open an issue](https://github.com/ahenyao/MeowPlayer/issues).


## License

This project is licensed under the PolyForm Noncommercial 1.0.0 License - see the [LICENSE](LICENSE) file for details.

This project uses third-party libraries - see the [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt) file for details.