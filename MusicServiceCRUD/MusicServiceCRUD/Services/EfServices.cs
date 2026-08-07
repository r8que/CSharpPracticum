using Microsoft.EntityFrameworkCore;
using MusicServiceCRUD.Data;
using MusicServiceCRUD.Models;

namespace MusicServiceCRUD.Services;

public class EfService
{
    private readonly MusicDbContext _context;

    public EfService(MusicDbContext context)
    {
        _context = context;
    }

    public List<Album> GetAllAlbums()
    {
        return _context.Albums.ToList();
    }

    public void AddAlbum(string title, DateTime releaseDate, decimal price, Guid artistId)
    {
        var album = new Album
        {
            AlbumId = Guid.NewGuid(),
            Title = title,
            ReleaseDate = releaseDate,
            Price = price,
            ArtistId = artistId
        };

        _context.Albums.Add(album);
        _context.SaveChanges();
    }

    public void UpdateAlbum(Guid id, string title, DateTime releaseDate, decimal price)
    {
        var album = _context.Albums.Find(id);
        if (album == null) return;

        album.Title = title;
        album.ReleaseDate = releaseDate;
        album.Price = price;

        _context.SaveChanges();
    }

    public void DeleteAlbum(Guid id)
    {
        var album = _context.Albums.Find(id);
        if (album == null) return;

        _context.Albums.Remove(album);
        _context.SaveChanges();
    }
}