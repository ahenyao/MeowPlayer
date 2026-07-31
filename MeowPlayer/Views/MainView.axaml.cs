using Avalonia.Controls;

namespace MeowPlayer.Views;

public partial class MainView : UserControl {
    public MainView() {
        InitializeComponent();
        
        Utils.Globals.Initialize(MeowPlayer_PlayerView, 
            MeowPlayer_LibraryView, 
            MeowPlayer_SettingsView, 
            MeowPlayer_TabControl);
    }
}