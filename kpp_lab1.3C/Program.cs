using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Composition
{
    public string Name { get; set; }
    public string Genre { get; set; }
    public string Artist { get; set; }
    public string Lyrics { get; set; }
    public DateTime CreationDate { get; set; }
    public TimeSpan Duration { get; set; }
    public string Format { get; set; }
    public double Rating { get; set; }

    public override string ToString()
    {
        return $"Composition(Iм'я='{Name}', Жанр='{Genre}', Артист='{Artist}', " +
            $"Текст='{Lyrics}',Дата створення='{CreationDate}', Тривалiсть='{Duration}', " +
            $"Формат='{Format}', Рейтинг='{Rating}')";
    }
}

class Program
{
    private static List<Composition> compositions = new List<Composition>();
    private const string FileName = "compositions.json";

    static void Main(string[] args)
    {
        LoadCompositions();

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Додайти нову композицiю");
            Console.WriteLine("2. Переглянути список композицiй");
            Console.WriteLine("3. Сортування композицiї за iм'ям");
            Console.WriteLine("4. Сортувати композицiї за виконавцем");
            Console.WriteLine("5. Сортувати композицiї за середнiм рейтингом");
            Console.WriteLine("6. Exit");
            Console.Write("Виберiть опцiю: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    AddComposition();
                    break;

                case 2:
                    DisplayCompositions();
                    break;

                case 3:
                    compositions = compositions.OrderBy(c => c.Name).ToList();
                    Console.WriteLine("Композицiї вiдсортованi за назвою.");
                    break;

                case 4:
                    compositions = compositions.OrderBy(c => c.Artist).ToList();
                    Console.WriteLine("Композицiї вiдсортованi за виконавцями.");
                    break;

                case 5:
                    compositions = compositions.OrderByDescending(c => c.Rating).ToList();
                    Console.WriteLine("Композицiї вiдсортованi за середнiм рейтингом.");
                    break;

                case 6:
                    SaveCompositions();
                    Console.WriteLine("Програму припинено.");
                    return;

                default:
                    Console.WriteLine("Невiрний вибiр. Спробуйте знову.");
                    break;
            }
        }
    }

    private static void LoadCompositions()
    {
        if (File.Exists(FileName))
        {
            try
            {
                var json = File.ReadAllText(FileName);
                compositions = JsonSerializer.Deserialize<List<Composition>>(json);
                Console.WriteLine("Данi успiшно завантажено compositions.json");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Помилка пiд час завантаження даних з compositions.json: {e.Message}.");
            }
        }
        else
        {
            Console.WriteLine("compositions.json не знайдено.");
        }
    }

    private static void AddComposition()
    {
        Console.WriteLine("Ведiть данi композицiї");

        Console.Write("Iм'я: ");
        var name = Console.ReadLine();

        Console.Write("Жанр: ");
        var genre = Console.ReadLine();

        Console.Write("Артист: ");
        var artist = Console.ReadLine();

        Console.Write("Текст: ");
        var lyrics = Console.ReadLine();

        Console.Write("Дата створення (yyyy-MM-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime creationDate))
        {
            Console.Write("Тривалiсть (hh:mm:ss): ");
            if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan duration))
            {
                Console.Write("Формат: ");
                var format = Console.ReadLine();

                Console.Write("Рейтинг: ");
                if (double.TryParse(Console.ReadLine(), out double rating))
                {
                    var composition = new Composition
                    {
                        Name = name,
                        Genre = genre,
                        Artist = artist,
                        Lyrics = lyrics,
                        CreationDate = creationDate,
                        Duration = duration,
                        Format = format,
                        Rating = rating
                    };

                    compositions.Add(composition);
                    Console.WriteLine("Композицiя додана");
                }
                else
                {
                    Console.WriteLine("Неправильний формат оцiнки.");
                }
            }
            else
            {
                Console.WriteLine("Неправильний формат тривалостi.");
            }
        }
        else
        {
            Console.WriteLine("Неправильний формат дати.");
        }
    }

    private static void DisplayCompositions()
    {
        foreach (var composition in compositions)
        {
            Console.WriteLine(composition);
        }
    }

    private static void SaveCompositions()
    {
        var json = JsonSerializer.Serialize(compositions, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
