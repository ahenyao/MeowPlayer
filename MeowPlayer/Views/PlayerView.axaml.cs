using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Reactive;
using Avalonia.Threading;
using MediaPlayer = ManagedBass.MediaPlayer;

namespace MeowPlayer.Views;

public partial class PlayerView : UserControl {
    private int _songId = 0;
    public MeowPlayer.Utils.MediaPlayer Player = new();
    
    
    public static readonly StyledProperty<long> SongPositionProperty =
        AvaloniaProperty.Register<PlayerView, long>(nameof(SongPosition));
    public long SongPosition
    {
        get => GetValue(SongPositionProperty);
        set => SetValue(SongPositionProperty, value);
    }

    public static readonly StyledProperty<long> SongDurationProperty =
        AvaloniaProperty.Register<PlayerView, long>(nameof(SongDuration));

    public long SongDuration
    {
        get => GetValue(SongDurationProperty);
        set => SetValue(SongDurationProperty, value);
    }
    
    private DispatcherTimer _updateSliderTimer;
    
    // this checks whether playback status was changed from android notification or play button
    bool IsPlaying {
        get {
            Img_SongAlbumArt.Background = new SolidColorBrush(
                field ? Color.FromRgb(0, 255, 255) : Color.FromRgb(255, 0, 255)
            );
            return field;
        }
        set {
            field = value;
            Img_SongAlbumArt.Background = new SolidColorBrush(
                value ? Color.FromRgb(0, 127, 127) : Color.FromRgb(127, 0, 127)
            );
        }
    }
    public PlayerView() {
        InitializeComponent();
        
        _updateSliderTimer = new DispatcherTimer {
            Interval = TimeSpan.FromMilliseconds(250)
        };
        
        _updateSliderTimer.Tick += (_, _) => {
            SongPosition = (long)Player.Position.TotalMilliseconds;
            Tb_SongDurationCurrent.Text = Utils.Player.CalcTimeFromMillis(SongPosition);
            //Logger.Log(Logger.LogLevel.DEBUG, $"Slider min/max/val/seek: {Sl_Position.Minimum}/{Sl_Position.Maximum}/{Sl_Position.Value}/{player.Position.TotalMilliseconds}");
        };
        
        _updateSliderTimer.Start();

        this.GetObservable(BoundsProperty).Subscribe(new AnonymousObserver<Rect>(bounds => {
            double calculatedSize = Math.Min(80.0, bounds.Width / 7.5);
            BtPlayMaxHeight = calculatedSize;
        }));
        
        PlatformMessaging.OnPlayRequested = () => {
            IsPlaying = true;
            Bt_Play.Content = "| |";
            Player.Play();
            PlatformMessaging.OnPlaybackStateChanged?.Invoke(true, 0); 
        };

        PlatformMessaging.OnPauseRequested = () => {
            IsPlaying = false;
            Bt_Play.Content = ">";
            Player.Pause();
            PlatformMessaging.OnPlaybackStateChanged?.Invoke(false, 0);
        };

        PlatformMessaging.OnSeekRequested = (targetMs) => {
            // Core audio loop jumping logic
            PlatformMessaging.OnPlaybackStateChanged?.Invoke(true, targetMs);
        };
        
        PlatformMessaging.OnNextRequested = NextSong; 
        PlatformMessaging.OnPreviousRequested = PrevSong;
    }

    public static readonly StyledProperty<double> BtPlayMaxHeightProperty = AvaloniaProperty.Register<PlayerView, double>(nameof(BtPlayMaxHeight), defaultValue: 80.0);

    public double BtPlayMaxHeight {
        get => GetValue(BtPlayMaxHeightProperty);
        set => SetValue<double>(BtPlayMaxHeightProperty, value);
    }
    private void Bt_Prev_OnClick(object? sender, RoutedEventArgs e) {
        PrevSong();
    }

    private void Bt_Play_OnClick(object? sender, RoutedEventArgs e) {
        IsPlaying = !IsPlaying;
        // Bt_Play.Content = !isPlaying ? ">" : "| |";
        PlatformMessaging.OnPlaybackStateChanged?.Invoke(IsPlaying, 0);
        if (IsPlaying) PlatformMessaging.OnPlayRequested?.Invoke();
        else PlatformMessaging.OnPauseRequested?.Invoke();
    }

    private void Bt_Next_OnClick(object? sender, RoutedEventArgs e) {
        NextSong();
    }


    private void Bt_Shuffle_OnClick(object? sender, RoutedEventArgs e) {
        // _ = meow();
    }

    private async void Bt_Repeat_OnClick(object? sender, RoutedEventArgs e) {
    }


    private void NextSong() {
        _songId++;
        _songId %= 5;
        Song($"meow {_songId}", "nya", _songId*1000);
    }

    private void PrevSong() {
        _songId--;
        _songId %= 5;
        Song($"meow {_songId}", "nya", _songId*1000);
    }

    public void Song(string title, string artist, TimeSpan ts) {
        Song(title, artist, (long)ts.TotalMilliseconds);
        SongDuration = (long)ts.TotalMilliseconds;
    }

    public void Song(string title, string artist, long duration) {
        Tb_SongDurationCurrent.Text = "0:00";

        Tb_SongDurationFull.Text = Utils.Player.CalcTimeFromMillis(duration);
        
        Tb_SongTitleArtist.Text = title + "\n" + artist;
        PlatformMessaging.OnTrackChanged?.Invoke(title, artist, duration);
    }
    
}