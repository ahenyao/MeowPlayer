using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media.Session;
using Avalonia.Android;

namespace MeowPlayer.Android;

[Activity(
    Label = "MeowPlayer.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity {
    protected override void OnStart() {
        base.OnStart();

        if (OperatingSystem.IsAndroidVersionAtLeast(33)) { // Android 13+ (new notification popup)
            if (CheckSelfPermission(global::Android.Manifest.Permission.PostNotifications) != Permission.Granted) {
                RequestPermissions([global::Android.Manifest.Permission.PostNotifications], 101);
            }
        }

        var intent = new Intent(this, typeof(AudioPlaybackService));
        if (OperatingSystem.IsAndroidVersionAtLeast(26)) { // Android 8.0+
            StartForegroundService(intent);
        } else { // Android up to 7.1.2
            StartService(intent);
        }

        PlatformMessaging.OnTrackChanged = (title, album, durationMs) => {
            if (AudioPlaybackService.Instance != null) {
                var context = global::Android.App.Application.Context;
                Bitmap? dummyArt = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.Icon);
        
                AudioPlaybackService.Instance.UpdateNotification(
                    title: title,
                    album: album,
                    context: context,
                    channelId: "meow_player_playback_channel",
                    albumArtBitmap: dummyArt,
                    durationMs: durationMs
                );
            }
        };

        PlatformMessaging.OnPlaybackStateChanged = (isPlaying, currentPositionMs) => {
            if (AudioPlaybackService.Instance != null) {
                var stateCode = isPlaying ? 
                    PlaybackStateCode.Playing : 
                    PlaybackStateCode.Paused;
                    
                float speedMultiplier = isPlaying ? 1.0f : 0.0f;

                AudioPlaybackService.Instance.UpdatePlaybackState(stateCode, currentPositionMs, speedMultiplier);
            }
        };
    }
}