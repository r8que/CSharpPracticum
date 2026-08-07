using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MusicServiceCRUD.Data;
using MusicServiceCRUD.Services;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = config.GetConnectionString("MusicService")!;

var options = new DbContextOptionsBuilder<MusicDbContext>()
    .UseSqlServer(connectionString)
    .Options;

using var context = new MusicDbContext(options);

var adoService = new AdoNetService(config);
var efService = new EfService(context);

while (true)
{
    Console.Clear();
    Console.WriteLine("=== MusicService CRUD ===");
    Console.WriteLine("--- ADO.NET (Artists) ---");
    Console.WriteLine("1. Показать артистов");
    Console.WriteLine("2. Добавить артиста");
    Console.WriteLine("3. Обновить артиста");
    Console.WriteLine("4. Удалить артиста");
    Console.WriteLine();
    Console.WriteLine("--- Entity Framework (Albums) ---");
    Console.WriteLine("5. Показать альбомы");
    Console.WriteLine("6. Добавить альбом");
    Console.WriteLine("7. Обновить альбом");
    Console.WriteLine("8. Удалить альбом");
    Console.WriteLine();
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1": ShowArtists(adoService); break;
        case "2": AddArtist(adoService); break;
        case "3": UpdateArtist(adoService); break;
        case "4": DeleteArtist(adoService); break;

        case "5": ShowAlbums(efService); break;
        case "6": AddAlbum(efService); break;
        case "7": UpdateAlbum(efService); break;
        case "8": DeleteAlbum(efService); break;

        case "0": return;
        default: Console.WriteLine("Неверный выбор"); break;
    }

    Console.WriteLine("\nНажми Enter...");
    Console.ReadLine();
}

// ADO.NET методы
void ShowArtists(AdoNetService service)
{
    var artists = service.GetAllArtists();
    Console.WriteLine("\nАртисты:");
    foreach (var a in artists)
        Console.WriteLine($"{a.ArtistId} | {a.Name} | {a.Country} | Verified: {a.IsVerified}");
}

void AddArtist(AdoNetService service)
{
    Console.Write("Имя: ");
    var name = Console.ReadLine()!;
    Console.Write("Страна: ");
    var country = Console.ReadLine();
    Console.Write("Верифицирован? (true/false): ");
    bool.TryParse(Console.ReadLine(), out bool isVerified);

    service.AddArtist(name, country, isVerified);
    Console.WriteLine("Артист добавлен!");
}

void UpdateArtist(AdoNetService service)
{
    Console.Write("ID артиста: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { Console.WriteLine("Некорректный ID"); return; }

    Console.Write("Новое имя: ");
    var name = Console.ReadLine()!;
    Console.Write("Новая страна: ");
    var country = Console.ReadLine();
    Console.Write("Верифицирован? (true/false): ");
    bool.TryParse(Console.ReadLine(), out bool isVerified);

    service.UpdateArtist(id, name, country, isVerified);
    Console.WriteLine("Артист обновлён!");
}

void DeleteArtist(AdoNetService service)
{
    Console.Write("ID артиста: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { Console.WriteLine("Некорректный ID"); return; }

    service.DeleteArtist(id);
    Console.WriteLine("Артист удалён!");
}

// EF методы
void ShowAlbums(EfService service)
{
    var albums = service.GetAllAlbums();
    Console.WriteLine("\nАльбомы:");
    foreach (var a in albums)
        Console.WriteLine($"{a.AlbumId} | {a.Title} | {a.ReleaseDate:yyyy-MM-dd} | {a.Price} | ArtistId: {a.ArtistId}");
}

void AddAlbum(EfService service)
{
    Console.Write("Название альбома: ");
    var title = Console.ReadLine()!;
    Console.Write("Дата выхода (гггг-мм-дд): ");
    DateTime.TryParse(Console.ReadLine(), out DateTime date);
    Console.Write("Цена: ");
    decimal.TryParse(Console.ReadLine(), out decimal price);
    Console.Write("ArtistId: ");
    Guid.TryParse(Console.ReadLine(), out Guid artistId);

    service.AddAlbum(title, date, price, artistId);
    Console.WriteLine("Альбом добавлен!");
}

void UpdateAlbum(EfService service)
{
    Console.Write("ID альбома: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { Console.WriteLine("Некорректный ID"); return; }

    Console.Write("Новое название: ");
    var title = Console.ReadLine()!;
    Console.Write("Новая дата (гггг-мм-дд): ");
    DateTime.TryParse(Console.ReadLine(), out DateTime date);
    Console.Write("Новая цена: ");
    decimal.TryParse(Console.ReadLine(), out decimal price);

    service.UpdateAlbum(id, title, date, price);
    Console.WriteLine("Альбом обновлён!");
}

void DeleteAlbum(EfService service)
{
    Console.Write("ID альбома: ");
    if (!Guid.TryParse(Console.ReadLine(), out Guid id)) { Console.WriteLine("Некорректный ID"); return; }

    service.DeleteAlbum(id);
    Console.WriteLine("Альбом удалён!");
}