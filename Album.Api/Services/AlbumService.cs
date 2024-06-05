using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Album.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Album.Api.Services
{
    public class AlbumService : IAlbumService 
    {
        private readonly AlbumContext _context;
        public AlbumService(AlbumContext context)
        {
            _context = context;
        }
        
        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }

        public async Task<List<Models.Album>> GetAllAlbums()
        {
           return await _context.Albums.ToListAsync();
        }

        public async Task<Models.Album> GetAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            return album;
        }

        public async Task<Models.Album> PutAlbum(int id, Models.Album album)
        {
            _context.Entry(album).State = EntityState.Modified;
            try
            {
                 await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    throw new Exception("Not found!");
                }
                else
                {
                    throw new Exception("throwing");
                }
            }

            return album;
        }

        public async Task<Models.Album> PostAlbum(Models.Album album)
        {
            _context.Albums.Add(album);
            await _context.SaveChangesAsync();
            return album;
        }

        public async Task DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                throw new Exception("Error 404 Not found");
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();
        }
    }
}