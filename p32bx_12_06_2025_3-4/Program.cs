using Newtonsoft.Json;

namespace p32bx_12_06_2025_3_4
{
    class Program
    {
        static readonly string apiKey = "ВАШ_API_КЛЮЧ_OMDBAPI";

        static async Task Main()
        {
            Console.Write("Введите название фильма: ");
            string title = Console.ReadLine();

            var film = await GetFilmInfoAsync(title);
            if (film != null && film.Response == "True")
            {
                Console.WriteLine($"Название: {film.Title}");
                Console.WriteLine($"Год: {film.Year}");
                Console.WriteLine($"Режиссер: {film.Director}");
                Console.WriteLine($"Жанр: {film.Genre}");
                Console.WriteLine($"Описание: {film.Plot}");
            }
            else
            {
                Console.WriteLine("Фильм не найден");
            }
        }

        static async Task<Film> GetFilmInfoAsync(string title)
        {
            string url = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Film>(json);
            }
        }
    }

    public class Film
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public string Plot { get; set; }
        public string Response { get; set; }
    }
}
