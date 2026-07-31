using System;

namespace MeowPlayer;

public static class PlatformMessaging {
    public static Action<string, string, long>? OnTrackChanged { get; set; }
    public static Action? OnPauseRequested { get; set; }
    public static Action? OnPlayRequested { get; set; }
    public static Action? OnNextRequested { get; set; }
    public static Action? OnPreviousRequested { get; set; }
    public static Action? OnFastForwardRequested { get; set; }
    public static Action? OnRewindRequested { get; set; }
    public static Action<long>? OnSeekRequested { get; set; }
    public static Action<long>? OnActionsConfigChanged { get; set; }
    public static Action<bool, long>? OnPlaybackStateChanged { get; set; }
}