# 📀 AutoRip2MKV

[![Build Status](https://github.com/gmoyle/AutoRip2MKV_3/workflows/CI/badge.svg)](https://github.com/gmoyle/AutoRip2MKV_3/actions)
[![Release](https://img.shields.io/github/v/release/gmoyle/AutoRip2MKV_3)](https://github.com/gmoyle/AutoRip2MKV_3/releases/latest)
[![.NET](https://img.shields.io/badge/.NET-6.0-blue)](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
[![License](https://img.shields.io/github/license/gmoyle/AutoRip2MKV_3)](LICENSE)

> **A modern, automated DVD/Blu-ray ripping solution built with .NET 6**

AutoRip2MKV is a complete rewrite of the original AutoIt-based AutoRip2MKV, now modernized with C# and .NET 6. This headless ripping application automates your entire disc library conversion process using MakeMKV for ripping and HandBrake for encoding.

## ✨ Features

### 🎬 **Complete Automation**
- **Zero-touch operation** - Set as your default DVD/Blu-ray handler for fully automated ripping
- **Auto-eject and wait** - Automatically ejects discs and waits for the next one
- **Batch processing** - Handle multiple discs without intervention

### 🔧 **Flexible Conversion**
- **Full quality MKV ripping** with chapters, forced subtitles, and Dolby 5.1
- **Optional MP4 conversion** using customizable HandBrake presets
- **Smart cleanup** - Option to delete intermediate MKV files after conversion
- **Configurable title filtering** - Skip extras and trailers automatically

### 🏗️ **Modern Architecture**
- **Dependency Injection** - Clean, testable architecture with IoC container
- **Async/Await patterns** - Non-blocking operations throughout
- **Comprehensive logging** - Structured logging with configurable levels
- **Progress tracking** - Real-time operation monitoring
- **Configuration validation** - Robust settings validation and error handling

### 🔒 **Enterprise Features**
- **Secure credential management** - Windows Credential Manager integration
- **Error handling** - Comprehensive exception handling with custom error types
- **Unit testing** - Full test coverage for core functionality
- **CI/CD pipeline** - Automated builds, tests, and releases

## 🚀 Quick Start

### Prerequisites
- **Windows 10/11** (required for Windows Forms and credential management)
- **[MakeMKV](https://www.makemkv.com/)** - Free for DVD, license required for Blu-ray
- **[HandBrake CLI](https://handbrake.fr/downloads2.php)** - For MP4 conversion (optional)
- **.NET 6.0 Runtime** (if using framework-dependent build)

### Installation

1. **Download the latest release:**
   ```bash
   # Download self-contained executable (recommended)
   curl -L -o AutoRip2MKV.exe https://github.com/gmoyle/AutoRip2MKV_3/releases/latest/download/AutoRip2MKV.exe
   ```

2. **Or build from source:**
   ```bash
   git clone https://github.com/gmoyle/AutoRip2MKV_3.git
   cd AutoRip2MKV_3
   dotnet build --configuration Release
   ```

### Basic Usage

1. **Configure your settings** through the application interface
2. **Set as default handler** for DVD/Blu-ray auto-play (optional)
3. **Insert a disc** and let AutoRip2MKV handle the rest!

## ⚙️ Configuration

### Directory Setup
- **Temp Directory**: Fast storage for intermediate files (SSD recommended)
- **Final Directory**: Long-term storage location for completed files
- **Both must be subfolders, not root drives**

### HandBrake Customization
Customize the `HandbrakeCMDLine` setting to match your target hardware. Include only the preset settings from the file extension onward:

**Examples:**
- **Universal**: `-e x264 -q 20 -B 160`
- **High Quality**: `-e x264 -q 18 -B 320`
- **Mobile Optimized**: `-e x264 -q 22 -B 128 -X 720`

For more presets, see [HandBrake's Built-in Presets](https://trac.handbrake.fr/wiki/BuiltInPresets)

### MakeMKV Profile
Recommended MakeMKV selection profile for English content:
```
-sel:all,+sel:subtitle,-100:(eng),-99:(forced*(eng)),-90:(eng),-89:(forced*(eng)),+sel:audio,-sel:special,-100:(audio*(eng)),-90:(audio*(eng))
```

### Title Length Filtering
The `MinimumTitleLength` setting (default: ~20 minutes) filters out extras and trailers. Adjust for:
- **TV Episodes**: Lower value (e.g., 15 minutes)
- **Movies Only**: Higher value (e.g., 45 minutes)

## 🏗️ Development

### Building
```bash
# Restore dependencies
dotnet restore

# Build solution
dotnet build --configuration Release

# Run tests
dotnet test

# Create self-contained executable
dotnet publish -c Release -r win-x64 --self-contained true --single-file
```

### Testing
```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test category
dotnet test --filter Category=Unit
```

### CI/CD
The project includes comprehensive GitHub Actions workflows:
- **Build & Test**: Validates all PRs and commits
- **Release**: Creates artifacts for releases
- **Code Quality**: SonarCloud integration (optional)
- **Security**: Vulnerability scanning

## 📋 Requirements

### System Requirements
- **OS**: Windows 10/11
- **Framework**: .NET 6.0
- **RAM**: 2GB minimum, 4GB recommended
- **Storage**: Fast temp drive (SSD) recommended for performance

### Dependencies
- **MakeMKV**: Core ripping functionality
- **HandBrake CLI**: MP4 conversion (optional)
- **Windows Credential Manager**: Secure credential storage

## 🤝 Contributing

Contributions are welcome! Please read our contributing guidelines:

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Commit** your changes (`git commit -m 'Add amazing feature'`)
4. **Push** to the branch (`git push origin feature/amazing-feature`)
5. **Open** a Pull Request

### Development Setup
- Install Visual Studio 2022 or VS Code with C# extension
- Install .NET 6.0 SDK
- Clone the repository and restore packages

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ⚠️ Disclaimer

**User assumes all risks and liabilities for use of this application.** This software is provided "as is" without warranty of any kind. Please ensure you have the legal right to rip any discs you process.

## 🙏 Acknowledgments

- **MakeMKV** team for the excellent ripping engine
- **HandBrake** team for the powerful encoding tools
- Original **AutoIt** version contributors
- The .NET community for excellent tooling and libraries

---

<div align="center">

**Made with ❤️ for disc preservation enthusiasts**

[Report Bug](https://github.com/gmoyle/AutoRip2MKV_3/issues) • [Request Feature](https://github.com/gmoyle/AutoRip2MKV_3/issues) • [Discussions](https://github.com/gmoyle/AutoRip2MKV_3/discussions)

</div>

