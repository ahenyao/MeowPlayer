using System;

namespace MeowPlayer.Utils;

public class Player {
    public static string CalcTimeFromMillis(long millis) {

        TimeSpan ts = TimeSpan.FromMilliseconds(millis);

        if (ts.TotalHours >= 1) {
            return $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
        } else {
            return $"{ts.Minutes:00}:{ts.Seconds:00}";
        }
    }
}