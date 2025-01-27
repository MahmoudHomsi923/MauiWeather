using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWeather.Converters
{
    public class CodeToWeatherConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is float code)
            {
                return code switch
                {
                    0 => "Clear sky",
                    1 => "Mainly clear",
                    2 => "Partly cloudy",
                    3 => "Overcast",
                    45 => "Fog",
                    48 => "Depositing rime fog",
                    51 => "Drizzle: Light",
                    53 => "Drizzle: Moderate",
                    55 => "Drizzle: Dense intensity",
                    56 => "Freezing Drizzle: Light",
                    57 => "Freezing Drizzle: Dense intensity",
                    61 => "Rain: Slight",
                    63 => "Rain: Moderate",
                    65 => "Rain: Heavy intensity",
                    66 => "Freezing Rain: Light",
                    67 => "Freezing Rain: Heavy intensity",
                    71 => "Snow fall: Slight",
                    73 => "Snow fall: Moderate",
                    75 => "Snow fall: Heavy intensity",
                    77 => "Snow grains",
                    80 => "Rain showers: Slight",
                    81 => "Rain showers: Moderate",
                    82 => "Rain showers: Violent",
                    85 => "Snow showers slight",
                    86 => "Snow showers heavy",
                    95 => "Thunderstorm: Slight or moderate",
                    96 => "Thunderstorm with slight and heavy hail",
                    99 => "Thunderstorm with slight and heavy hail",
                    _ => "Unknown"
                };
            }

            return "Unknown";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
