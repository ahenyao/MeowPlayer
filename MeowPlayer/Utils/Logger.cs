using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace MeowPlayer.Utils;

public static class Logger {

    public enum LogLevel {
        TRACE, DEBUG, INFO, WARN, ERROR, FATAL
    };
    
    private static readonly object _lock = new();

    private static List<string> _bufferLog = new();
    private static List<string> _bufferErrorLog = new();

    public static string? LogFile {
        get;
        set {
            lock (_lock) {
                field = value;
                if (string.IsNullOrEmpty(field) || _bufferLog.Count <= 0) return;
                File.AppendAllLines(field, _bufferLog);
                _bufferLog.Clear();
            }
        }
    }

    public static string? ErrorLogFile {
        get;
        set {
            lock (_lock) {
                field = value;
                if (string.IsNullOrEmpty(field) || _bufferErrorLog.Count <= 0) return;
                File.AppendAllLines(field, _bufferErrorLog);
                _bufferErrorLog.Clear();
            }
        }
    }

    public static void Log(LogLevel level, 
        string message, 
        [CallerMemberName] string memberName = "", 
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0) 
    {
        
        string fileName = Path.GetFileName(filePath); 
        
        string colorCode = level switch {
            LogLevel.TRACE => "\u001b[32m",
            LogLevel.DEBUG => "\u001b[36m",
            LogLevel.INFO => "\u001b[37m",
            LogLevel.WARN => "\u001b[33m",
            LogLevel.ERROR => "\u001b[31m",
            LogLevel.FATAL => "\u001b[101m\u001b[97m",
            _ => "\u001b[37m"
        };
        string resetCode = "\u001b[0m";
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        
        string msg = $"[{timestamp}] [{level, -5}] [{fileName}/{memberName}/{lineNumber}]: {message}";


        bool isError = (level is LogLevel.ERROR or LogLevel.FATAL);
        TextWriter stream = isError ? Console.Error : Console.Out;
        bool useColor = isError switch {
            true when Console.IsErrorRedirected => false,
            false when Console.IsOutputRedirected => false,
            _ => true
        };

        
        lock (_lock) {

            if (useColor) stream.WriteLine(colorCode + msg + resetCode);
            else stream.WriteLine(msg);
        
            if (!string.IsNullOrEmpty(LogFile))
                File.AppendAllText(LogFile, msg + Environment.NewLine);
            else _bufferLog.Add(msg);

            if (isError) {
                if (!string.IsNullOrEmpty(ErrorLogFile))
                    File.AppendAllText(ErrorLogFile, msg + Environment.NewLine);
                else _bufferErrorLog.Add(msg);
            }
        }
    }
}