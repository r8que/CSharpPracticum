using Microsoft.EntityFrameworkCore;
using MusicServiceCRUD.Models;

namespace MusicServiceCRUD.Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;
    public DbSet<Track> Tracks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}