using Newtonsoft.Json;

namespace p32bx12_06_2025_2
{
    class Program
    {
        static readonly string apiKey = "ВАШ_API_КЛЮЧ_OPENWEATHERMAP";

        static async Task Main()
        {
            Console.Write("Введите название города: ");
            string city = Console.ReadLine();

            var coords = await GetCoordinatesAsync(city);
            if (coords != null)
            {
                Console.WriteLine($"Координаты: Широта {coords.Lat}, Долгота {coords.Lon}");

                
                var forecast = await GetWeeklyForecastAsync(coords.Lat, coords.Lon);
                if (forecast != null)
                {
                    Console.WriteLine($"\nПрогноз на 7 дней для {city}:\n");
                    int dayCount = 1;
                    foreach (var day in forecast.Daily)
                    {
                        DateTime date = DateTimeOffset.FromUnixTimeSeconds(day.Dt).DateTime;
                        Console.WriteLine($"День {dayCount++} - {date.ToString("dd MMM yyyy")}");
                        Console.WriteLine($"Температура: {day.Temp.Day - 273.15:F1}°C");
                        Console.WriteLine($"Описание: {day.Weather[0].Description}");
                        Console.WriteLine(new string('-', 30));
                    }
                }
            }
            else
            {
                Console.WriteLine("Не удалось определить координаты города");
            }
        }

        static async Task<Coordinates> GetCoordinatesAsync(string city)
        {
            string url = $"http://api.openweathermap.org/geo/1.0/direct?q={Uri.EscapeDataString(city)}&limit=1&appid={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(url);
                var locations = JsonConvert.DeserializeObject<GeoLocation[]>(json);
                if (locations.Length > 0)
                {
                    return new Coordinates { Lat = locations[0].Lat, Lon = locations[0].Lon };
                }
                return null;
            }
        }

        static async Task<Forecast> GetWeeklyForecastAsync(double lat, double lon)
        {
            string url = $"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&exclude=current,minutely,hourly,alerts&appid={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Forecast>(json);
            }
        }
    }

   
    public class GeoLocation
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
        
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public class Forecast
    {
        public Daily[] Daily { get; set; }
    }

    public class Daily
    {
        public long Dt { get; set; }
        public Temperature Temp { get; set; }
        public Weather[] Weather { get; set; }
    }

    public class Temperature
    {
        public double Day { get; set; }
        
    }

    public class Weather
    {
        public string Description { get; set; }
    }
}
