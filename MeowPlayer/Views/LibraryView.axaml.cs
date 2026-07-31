using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using MeowPlayer.Utils;

namespace MeowPlayer.Views;

public partial class LibraryView : UserControl {
    
    public LibraryView() {
        InitializeComponent();
    }

    private async void ButtonSong_OnClick(object? sender, RoutedEventArgs e) {
        var topLevel = TopLevel.GetTopLevel(this);

        if (topLevel == null) {
            Logger.Log(Logger.LogLevel.FATAL, "TopLevel was null");
            return;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions {
            Title = "Open audio file",
            AllowMultiple = false
        });

        if (files.Count >= 1) {
            var file = files[0];
            // string path = file.Path.LocalPath;
            
            Stream filestream = await file.OpenReadAsync();

            if (Globals.PlayerView == null) {
                Logger.Log(Logger.LogLevel.FATAL, "PlayerView was null");
                return;
            }
            
            if (Globals.TabControl == null) {
                Logger.Log(Logger.LogLevel.FATAL, "TabControl was null");
                return;
            }
                
            
            if (await Globals.PlayerView.Player.LoadAsync(filestream)) {
                Globals.PlayerView.Song(Globals.PlayerView.Player.Title, Globals.PlayerView.Player.Artist, Globals.PlayerView.Player.Duration);
                Globals.TabControl.SelectedIndex = 0;
                Globals.PlayerView.Player.Play();
            }
        }
    }
    
    private async void ButtonDir_OnClick(object? sender, RoutedEventArgs e) {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) {
            Logger.Log(Logger.LogLevel.FATAL, "TopLevel was null");
            return;
        }
    
        var dirs = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions {
            Title = "Meow",
            AllowMultiple = false
        });
        
        if (dirs.Count < 1) return;    
        
        IStorageFolder selectedFolder = dirs[0];
        Tb_files.Text = $"{dirs[0].Path}";
        
        var items = selectedFolder.GetItemsAsync();
        await foreach (var item in items) {
            if (item is IStorageFile file)
                Tb_files.Text += "\nFILE " + file.Name;
            else if (item is IStorageFolder folder)
                Tb_files.Text += "\nDIR  " + folder.Name;
        }
    }
    
}