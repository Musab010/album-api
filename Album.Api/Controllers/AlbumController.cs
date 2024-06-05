using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Album.Api.Models;
using Album.Api.Services;

namespace Album.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly AlbumContext _context;
        private readonly IAlbumService _service;

        public AlbumController(AlbumContext context, IAlbumService service)
        {
            _context = context;
            _service = service;
        }
        
        // GET: api/Album
        [HttpGet]
        public async Task<List<Models.Album>> GetAlbums()
        {
            return await _service.GetAllAlbums();
        }

        // GET: api/Album/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Album>> GetAlbum(int id)
        {
            var album = _service.GetAlbum(id);

            if (album == null)
            {
                return NotFound();
            }
            
            return await album;
        }

        // PUT: api/Album/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(int id, Models.Album album)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Album
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Album>> PostAlbum(Models.Album album)
        {
            await _service.PostAlbum(album);

            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        // DELETE: api/Album/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                await _service.DeleteAlbum(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
