using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MeowPlayer.Views;
using Avalonia.Styling;
using System.Runtime.InteropServices;
using Logger = MeowPlayer.Utils.Logger;

namespace MeowPlayer;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
        Current?.RequestedThemeVariant = ThemeVariant.Light;
    }

    public override void OnFrameworkInitializationCompleted() {

        Logger.Log(Logger.LogLevel.INFO, $"Compiled on:  {BuildInfo.BuildHostOS} ({BuildInfo.BuildHostArch})");
        Logger.Log(Logger.LogLevel.INFO, $"Running on:   {RuntimeInformation.OSDescription} ({RuntimeInformation.OSArchitecture})");
        Logger.Log(Logger.LogLevel.INFO, $"Process info: {RuntimeInformation.ProcessArchitecture}; {RuntimeInformation.FrameworkDescription}; {RuntimeInformation.RuntimeIdentifier}");
        Utils.Platform.AssignLogFiles();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new MainWindow();
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform) {
            singleViewPlatform.MainView = new MainView();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
