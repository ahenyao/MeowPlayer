using Avalonia.Controls;
using MeowPlayer.Views;

namespace MeowPlayer.Utils;

public static class Globals {
    public static PlayerView? PlayerView;
    public static LibraryView? LibraryView;
    public static SettingsView? SettingsView;
    public static TabControl? TabControl;

    public static void Initialize(
    PlayerView playerView, 
    LibraryView libraryView, 
    SettingsView settingsView, 
    TabControl tabControl) {
        PlayerView = playerView;
        LibraryView = libraryView;
        SettingsView = settingsView;
        TabControl = tabControl;
    }
}