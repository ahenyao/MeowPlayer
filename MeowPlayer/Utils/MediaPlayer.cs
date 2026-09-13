using System;
using System.IO;
using System.Threading.Tasks;
using ManagedBass;

namespace MeowPlayer.Utils;

public class MediaPlayer : ManagedBass.MediaPlayer {
    // ManagedBass library doesn't have loading from stream method.
    // Created for Android because it doesn't give real path but URI, and also file stream which we use here.

    private byte[]? _buffer;
    
    public async Task<bool> LoadAsync(Stream stream, string title = "<unknown>") {
        _buffer = new byte[stream.Length];
        if (stream.CanSeek) {
            stream.Position = 0;
        }
        await stream.ReadExactlyAsync(_buffer, 0, _buffer.Length);

        return await LoadAsync(title);
    }
    
    protected override int OnLoad(string fileName) {
        int handle = 0;
        
        if (_buffer != null)
            handle = Bass.CreateStream(_buffer, 0, _buffer.Length, BassFlags.Default);
        else
            handle = base.OnLoad(fileName);
        return handle;
    }
    
    
    public override void Dispose() {
        base.Dispose();
        _buffer = null;
    }
}
