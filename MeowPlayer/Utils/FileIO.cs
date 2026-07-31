using System;
using System.IO;

namespace MeowPlayer.Utils;

public static class FileIO {

    public struct FilePath(string value) {
        
        public string Value { get; } = value ?? "";

        public static FilePath operator /(FilePath a, string b) => new FilePath(Path.Combine(a.Value, b));
        
        public static FilePath operator /(FilePath a, FilePath b) => new FilePath(Path.Combine(a.Value, b.Value));
        
        public static implicit operator FilePath(string path) => new FilePath(path);    // from string to FilePath
        public static implicit operator string(FilePath path) => path.Value;            // from FilePath to string
        
        public override string ToString() => Value;
    }
    
    
    public static void SafeMkdir(string dirName) {
        if (File.Exists(dirName)) {
            File.Move(dirName, $"{dirName}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.bak");
        }
        
        if (!Directory.Exists(dirName)) {
            try {
                Directory.CreateDirectory(dirName);
            }
            catch (Exception ex) {
                Logger.Log(Logger.LogLevel.FATAL, ex.ToString());
            }
        }
    }
}