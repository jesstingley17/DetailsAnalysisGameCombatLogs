using System;
using System.Globalization;
using System.Windows.Data;

namespace CombatAnalysis.App.Converters.CombatLogs;

public class IntToEnhancedIntConverter : IValueConverter
{
    object IValueConverter.Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        double moreThanMillion = Convert.ToDouble(value) / 1000000;
        double enchance;
        string result;
        if (moreThanMillion < 1)
        {
            double moreThanThousand = (int)value / 1000;
            enchance = Math.Round(moreThanThousand, 3);
            result = $"{enchance}K";
        }
        else
        {
            enchance = Math.Round(moreThanMillion, 3);
            result = $"{enchance}M";
        }

        return result;
    }

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = int.Parse((string)value);

        return result;
    }
}