using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia;

namespace MeowPlayer.Converters;

public class MathMultiplierConverter : IValueConverter {
    public static readonly MathMultiplierConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {

        if (value is double baseValue && parameter != null) {
            string[] parameters = parameter.ToString()!.Split(',');

            if (parameters.Length == 1) {
                if (double.TryParse(parameter.ToString(), CultureInfo.InvariantCulture, out double multiplier)) {
                    return baseValue * multiplier;
                }
            }
            else if (parameters.Length == 2) {
                if (double.TryParse(parameters[0], CultureInfo.InvariantCulture, out double multiplier) && double.TryParse(parameters[1], CultureInfo.InvariantCulture, out double add)) {
                    return baseValue * multiplier + add;
                }
            }
        }

        return value ?? 0;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }
}