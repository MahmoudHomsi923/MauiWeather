using MauiWeather.MVVM.Models;
using PropertyChanged;
using System.Text.Json;
using System.Windows.Input;

namespace MauiWeather.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WeatherViewModel
    {
        // Eigenschaften für die Bindung an die Ansicht
        public WeatherData WeatherData { get; set; }
        public string PlaceName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsVisible { get; set; }
        public bool IsLoading { get; set; }

        // HttpClient-Instanz für API-Anfragen
        private HttpClient client = new HttpClient();

        // Konstruktor
        public WeatherViewModel()
        {
            client = new HttpClient();
        }

        // Befehl für die Suchfunktionalität
        public ICommand SearchCommand =>
            new Command(async (searchText) =>
            {
                // Setze den Ortsnamen auf den Suchtext
                PlaceName = searchText.ToString();

                // Hole die Koordinaten für den angegebenen Ortsnamen
                var location = await GetCoordinatesAsync(searchText.ToString());

                // Hole die Wetterdaten für den erhaltenen Standort
                await GetWeather(location);
            });

        /// <summary>
        /// Abrufen der Wetterdaten für einen bestimmten Standort und in WeatherData speichern
        /// </summary>
        /// <param name="location">Typ Location</param>
        private async Task GetWeather(Location location)
        {
            // Erstelle die API-URL mit den Standortkoordinaten
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=weathercode,temperature_2m_max,temperature_2m_min&current_weather=true&timezone=America%2FChicago";

            // Setze den Ladezustand auf true
            IsLoading = true;

            // Führe die API-Anfrage aus
            var response = await client.GetAsync(url);

            // Überprüfe, ob die Antwort erfolgreich ist
            if (response.IsSuccessStatusCode)
            {
                // Deserialisiere den Antwortstream in ein WeatherData-Objekt
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<WeatherData>(responseStream);

                    WeatherData = data;

                    // Fülle die daily2-Sammlung mit den täglichen Wetterdaten
                    for (int i = 0; i < WeatherData.daily.time.Length; i++)
                    {
                        var daily2 = new Daily2
                        {
                            time = WeatherData.daily.time[i],
                            temperature_2m_max = WeatherData.daily.temperature_2m_max[i],
                            temperature_2m_min = WeatherData.daily.temperature_2m_min[i],
                            weathercode = WeatherData.daily.weathercode[i]
                        };
                        WeatherData.daily2.Add(daily2);
                    }
                    // Setze die Sichtbarkeit auf true, um die Wetterdaten anzuzeigen
                    IsVisible = true;
                }
            }
            // Setze den Ladezustand auf false
            IsLoading = false;
        }

        /// <summary>
        /// Abrufen der Koordinaten für eine bestimmte Adresse
        /// </summary>
        /// <param name="address">Typ String</param>
        /// <returns>Typ Task<Location></returns>
        private async Task<Location?> GetCoordinatesAsync(string address)
        {
            // Verwende die Geocoding-API, um Standorte für die angegebene Adresse zu erhalten
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);

            // Hole den ersten Standort aus dem Ergebnis
            Location? location = locations?.FirstOrDefault();

            // Protokolliere die Koordinaten, wenn ein Standort gefunden wurde
            if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");

            return location;
        }
    }
}