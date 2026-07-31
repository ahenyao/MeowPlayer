using System;
using System.IO;

namespace MeowPlayer.Utils;

public static class Platform {

    private static readonly object _lock = new();
    private static bool _logsAssigned = false;
    
    
    public static bool LINUX => OperatingSystem.IsLinux();
    public static bool MACOS => OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst();
    public static bool WINDOWS => OperatingSystem.IsWindows();
    public static bool IOS => OperatingSystem.IsIOS();
    public static bool ANDROID => OperatingSystem.IsAndroid();
    public static bool DESKTOP => LINUX || MACOS || WINDOWS;
    public static bool MOBILE => IOS || ANDROID;
    public static bool OTHER => !DESKTOP && !MOBILE;

    public static readonly FileIO.FilePath AppDir = new FileIO.FilePath(
        true switch {
            _ when WINDOWS => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),     //   %appdata%
            _ when MACOS => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),       //   ~/Library/Application Support
            _ when LINUX => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),       //   ~/.config
            _ when IOS => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),         //   (On my iPhone)
            _ when ANDROID => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),     //   /storage/emulated/0/Android/data
            _ => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        }) / "MeowPlayer";
    

    public static FileIO.FilePath LogsDir = AppDir/"Logs";

    static Platform() {
        FileIO.SafeMkdir(AppDir);
        FileIO.SafeMkdir(AppDir/"Logs");
        FileIO.SafeMkdir(AppDir/"Config");
        
        FileIO.SafeMkdir(AppDir/"Library");
        FileIO.SafeMkdir(AppDir/"Library"/"Playlists");
        
        FileIO.SafeMkdir(AppDir/"Cache");
        FileIO.SafeMkdir(AppDir/"Cache"/"Artwork");
    }


    public static void AssignLogFiles() {
        if (Avalonia.Controls.Design.IsDesignMode) return;
        lock (_lock) {
            if (_logsAssigned) return;
            
            int i = 1;

            string format = $"MeowPlayer_{DateTime.Now:yyyy-MM-dd}_";

            if (Directory.Exists(LogsDir)) {

                // this checks if _1 log files exist and if they do it checks _2 and etc
                while (
                    File.Exists(LogsDir/$"{format}{i}.log") ||
                    File.Exists(LogsDir/$"{format}{i}.error.log")
                ) i++;

                Logger.LogFile = LogsDir/$"{format}{i}.log";
                Logger.ErrorLogFile = LogsDir/$"{format}{i}.error.log";
                _logsAssigned = true;
            }
        }
    }
}