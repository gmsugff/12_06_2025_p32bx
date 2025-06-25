using Newtonsoft.Json;
using System.Net.Mail;

namespace p32bx_12_06_2025_3_4
{
    class Program
    {
        
        static readonly string omdbApiKey = "ВАШ_API_КЛЮЧ_OMDBAPI";

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

               
                string resultText = $"Информация о фильме:\n" +
                                    $"Название: {film.Title}\n" +
                                    $"Год: {film.Year}\n" +
                                    $"Режиссер: {film.Director}\n" +
                                    $"Жанр: {film.Genre}\n" +
                                    $"Описание: {film.Plot}";

                string filePath = "FilmInfo.txt";
                File.WriteAllText(filePath, resultText);
                Console.WriteLine($"Информация сохранена в файл {filePath}");

                
                Console.Write("Хотите отправить результаты по email? (да/нет): ");
                string answer = Console.ReadLine().ToLower();
                if (answer == "да" || answer == "yes" || answer == "д")
                {
                    Console.Write("Введите email получателя: ");
                    string email = Console.ReadLine();

                    Console.Write("Введите тему письма: ");
                    string subject = Console.ReadLine();

                    Console.Write("Введите текст письма: ");
                    string body = Console.ReadLine();

                    try
                    {
                        SendEmail(email, subject, body, filePath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Фильм не найден или произошла ошибка");
            }
        }

        static async Task<Film> GetFilmInfoAsync(string title)
        {
            string url = $"http://www.omdbapi.com/?t={Uri.EscapeDataString(title)}&apikey={omdbApiKey}";
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Film>(json);
            }
        }

        static void SendEmail(string toAddress, string subject, string body, string attachmentPath)
        {
            MailMessage mail = new MailMessage("ваш_email@gmail.com", toAddress, subject, body);
            mail.Attachments.Add(new Attachment(attachmentPath));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("ваш_email@gmail.com", "ваш_пароль"),
                EnableSsl = true
            };

            smtp.Send(mail);
            Console.WriteLine("Письмо успешно отправлено");
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
