using System;
using Android.App;
using Android.Content;
using Android.Media.Session;
using Android.Graphics;
using Android.Media;
using Android.OS;
using MeowPlayer.Views;

namespace MeowPlayer.Android;

[Service(Enabled = true, Exported = false, ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeMediaPlayback)]
public partial class AudioPlaybackService : Service {
    
    public static AudioPlaybackService? Instance { get; private set; }
    private MediaSession? _mediaSession;
    private const int NOTIFICATION_ID = 1337;
    private const string CHANNEL_ID = "meowplayer_playback_channel";
    
    private const long DEFAULT_ACTIONS = PlaybackState.ActionPlay | 
                                        PlaybackState.ActionPause | 
                                        PlaybackState.ActionPlayPause | 
                                        PlaybackState.ActionSkipToNext | 
                                        PlaybackState.ActionSkipToPrevious | 
                                        PlaybackState.ActionSeekTo | 
                                        PlaybackState.ActionStop;
    
    public override void OnCreate() {
        base.OnCreate();
        Instance = this;
        CreateNotificationChannel();

        _mediaSession = new MediaSession(this, "MeowPlayerSession") { Active = true };
        _mediaSession.SetCallback(new MediaCallback(this));

        UpdatePlaybackState(PlaybackStateCode.Playing, 0, 1.0f);

        PlatformMessaging.OnActionsConfigChanged = (customActions) => {
            UpdatePlaybackState(PlaybackStateCode.Playing, 0, 1.0f, customActions);
        };
    }

    public void UpdatePlaybackState(PlaybackStateCode state, long positionMs, float speed, long actions = DEFAULT_ACTIONS) {
        var stateBuilder = new PlaybackState.Builder();
        stateBuilder.SetActions(actions);
        stateBuilder.SetState(state, positionMs, speed);
        _mediaSession?.SetPlaybackState(stateBuilder.Build());
    }

    public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId) {
        if (OperatingSystem.IsAndroidVersionAtLeast(26)) { // Android 8.0+
            Notification initialNotification = new Notification.Builder(this, CHANNEL_ID)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle("MeowPlayer")
                .SetContentText("Player initialized")
                .SetStyle(new Notification.MediaStyle().SetMediaSession(_mediaSession?.SessionToken))
                .Build();

            StartForeground(NOTIFICATION_ID, initialNotification);
            return StartCommandResult.Sticky;
        }

        return base.OnStartCommand(intent, flags, startId);
    }

    public void UpdateNotification(string title, string album, Context context, string channelId, Bitmap? albumArtBitmap, long durationMs) {
        var metadataBuilder = new MediaMetadata.Builder();
        metadataBuilder.PutString(MediaMetadata.MetadataKeyTitle, title);
        metadataBuilder.PutString(MediaMetadata.MetadataKeyArtist, album);
        metadataBuilder.PutBitmap(MediaMetadata.MetadataKeyAlbumArt, albumArtBitmap);
        metadataBuilder.PutLong(MediaMetadata.MetadataKeyDuration, durationMs);
        
        var metadata = metadataBuilder.Build();
        _mediaSession?.SetMetadata(metadata);

        Notification notification;
        
        if (OperatingSystem.IsAndroidVersionAtLeast(26)) { // Android 8.0+
            notification = new Notification.Builder(context, channelId)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle(title)
                .SetContentText(album)
                .SetLargeIcon(albumArtBitmap)
                .SetVisibility(NotificationVisibility.Public)
                .SetStyle(new Notification.MediaStyle().SetMediaSession(_mediaSession?.SessionToken))
                .Build();
        } else {
            notification = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.Icon)
                .SetContentTitle(title)
                .SetContentText(album)
                .SetLargeIcon(albumArtBitmap)
                .SetVisibility(NotificationVisibility.Public)
                .SetStyle(new Notification.MediaStyle().SetMediaSession(_mediaSession?.SessionToken))
                .Build();
        }
        
        StartForeground(NOTIFICATION_ID, notification);
    }

    public override IBinder? OnBind(Intent? intent) => null;

    private void CreateNotificationChannel() {
        if (OperatingSystem.IsAndroidVersionAtLeast(26)) { // Android 8.0+
            var channel = new NotificationChannel(CHANNEL_ID, "Audio Playback", NotificationImportance.Low) {
                Description = "Background audio playback notification."
            };
            var manager = (NotificationManager)GetSystemService(NotificationService)!;
            manager.CreateNotificationChannel(channel);
        }
    }

    public override void OnDestroy() {
        if (Instance == this) Instance = null;
        _mediaSession?.Release();
        base.OnDestroy();
    }
}