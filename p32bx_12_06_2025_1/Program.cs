using Newtonsoft.Json;

namespace p32bx_12_06_2025
{

    class Program
    {
        static readonly string apiKey = "ВАШ_API_КЛЮЧ_OPENWEATHERMAP";

        static async Task Main()
        {
            Console.Write("Введите название города: ");
            string city = Console.ReadLine();

            var weather = await GetWeatherAsync(city);
            if (weather != null)
            {
                Console.WriteLine($"Погода в {city}:");
                Console.WriteLine($"Температура: {weather.Main.Temp - 273.15:F1}°C");
                Console.WriteLine($"Описание: {weather.Weather[0].Description}");
            }
            else
            {
                Console.WriteLine("Не удалось получить данные о погоде");
            }
        }

        static async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WeatherResponse>(json);
                }
                return null;
            }
        }
    }

    public class WeatherResponse
    {
        public Weather[] Weather { get; set; }
        public Main Main { get; set; }
    }

    public class Weather
    {
        public string Description { get; set; }
    }

    public class Main
    {
        public double Temp { get; set; }
    }
}
