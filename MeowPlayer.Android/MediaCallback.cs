using Android.Media.Session;

namespace MeowPlayer.Android;

public class MediaCallback : MediaSession.Callback {
    private readonly AudioPlaybackService _service;

    public MediaCallback(AudioPlaybackService service) => _service = service;
    
    public override void OnPlay() => PlatformMessaging.OnPlayRequested?.Invoke();
    public override void OnPause() => PlatformMessaging.OnPauseRequested?.Invoke();
    public override void OnSkipToNext() => PlatformMessaging.OnNextRequested?.Invoke();
    public override void OnSkipToPrevious() => PlatformMessaging.OnPreviousRequested?.Invoke();
    public override void OnFastForward() => PlatformMessaging.OnFastForwardRequested?.Invoke();
    public override void OnRewind() => PlatformMessaging.OnRewindRequested?.Invoke();
    public override void OnSeekTo(long pos) => PlatformMessaging.OnSeekRequested?.Invoke(pos);
}