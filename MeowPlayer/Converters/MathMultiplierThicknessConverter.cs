using Avalonia.Data.Converters;
using System;
using System.Globalization;
using Avalonia;

namespace MeowPlayer.Converters;

public class MathMultiplierThicknessConverter : IValueConverter {
    public static readonly MathMultiplierThicknessConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {

        if (value is double baseValue && parameter != null) {
            string[] parameters = parameter.ToString()!.Split(',');

            if (parameters.Length == 6) {
                if (double.TryParse(parameters[0], CultureInfo.InvariantCulture, out double multiplier) && double.TryParse(parameters[1], CultureInfo.InvariantCulture, out double add)) {
                    double x = baseValue * multiplier;

                    double[] list = new double[4];
                    
                    for(int i = 2; i < 6; i++) {
                        if (parameters[i].Trim() == "x") {
                            list[i - 2] = x;
                        } else  if (parameters[i].Trim() == "-x") {
                            list[i - 2] = -x;
                        } else if (double.TryParse(parameters[i], CultureInfo.InvariantCulture, out double y)) {
                                list[i - 2] = y;
                        }
                    }

                    Thickness result = new Thickness(list[0], list[1], list[2], list[3]);
                    return result;
                }
            }
        }

        return new Thickness(0);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }
}