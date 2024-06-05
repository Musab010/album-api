using System.Linq;

namespace Album.Api.Models
{
    public class DbInitializer
    {
        public static void Initialize(AlbumContext context)
        {
            context.Database.EnsureCreated();
            
            if (context.Albums.Any())
            {
                return;
            }

            var albums = new Album[]
            {
                new Album{Artist = "Musab", Id = 1, ImageUrl = "www.google.nl/image1", Name = "Sivrikaya"},
                new Album{Artist = "John", Id = 2, ImageUrl = "www.google.nl/image2", Name = "Doe"},
                new Album{Artist = "Jane", Id = 3, ImageUrl = "www.google.nl/image3", Name = "Doe"},
                new Album{Artist = "Erik", Id = 4, ImageUrl = "www.google.nl/image4", Name = "Janssen"},
            };
            foreach (var album in albums)
            {
                context.Add(album);
            }

            context.SaveChanges();

        }
    }
}