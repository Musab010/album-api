using Microsoft.EntityFrameworkCore;


namespace Album.Api.Models
{
    public class AlbumContext : DbContext
    {
        public AlbumContext(DbContextOptions<AlbumContext> options) : base(options){ }
        public DbSet<Album> Albums { get; set; }
    }

    public class Album
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Artist { get; set; }
        public string ImageUrl { get; set; }
    }
}