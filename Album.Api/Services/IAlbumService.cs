using System.Collections.Generic;
using System.Threading.Tasks;
using Album.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Album.Api.Services
{
    public interface IAlbumService
    {
        // IEnumerable<Models.Album> GetAllAlbums();
        public Task< List<Models.Album>> GetAllAlbums();
        public Task<Models.Album> GetAlbum(int id);

        public Task<Models.Album> PutAlbum(int id, Models.Album album);
        public Task<Models.Album> PostAlbum(Models.Album album);

        public Task DeleteAlbum(int id);

    }
}