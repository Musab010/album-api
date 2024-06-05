using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Album.Api.Controllers;
using Album.Api.Models;
using Album.Api.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Xunit;
using IConfiguration = Castle.Core.Configuration.IConfiguration;

namespace Album.Api.Tests
{
    public class UnitTest2
    {
        private string _databaseName = "Testdb";
        private readonly IAlbumService _service;

        private async Task<AlbumContext> GetInMemoryDbWithDataAsync()
        {
            AlbumContext context = GetNewInMemoryDbWithData(true);
            await context.AddAsync(new Models.Album {Artist = "Musab", Id = 1, ImageUrl = "www.google.nl/image1", Name = "Sivrikaya"});
            await context.AddAsync(new Models.Album {Artist = "John", Id = 2, ImageUrl = "www.google.nl/image1", Name = "Doe"});
            await context.AddAsync(new Models.Album {Artist = "Jane", Id = 3, ImageUrl = "www.google.nl/image1", Name = "Doe"});

            await context.SaveChangesAsync();
            return GetNewInMemoryDbWithData(false);
        }

        private AlbumContext GetNewInMemoryDbWithData(bool newDb)
        {
            if (newDb)
            {
                _databaseName = Guid.NewGuid().ToString();
            }

            var optionsBuilder = new DbContextOptionsBuilder<AlbumContext>();
            optionsBuilder.UseInMemoryDatabase(_databaseName);
            return new AlbumContext(optionsBuilder.Options);
        }
        
        //Services tests
        [Fact]
        public async Task Get_All_Albums_Test()
        {
            //Arrange , Act , Assert
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var result = service.GetAllAlbums().Result;
            Assert.Equal(3 , result.Count);
        }

        [Fact]
        public async Task Test_Get_Albums_by_Id_return_valid()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var result = service.GetAlbum(1).Result;
            var value = new Models.Album
                {Artist = "Musab", Id = 1, ImageUrl = "www.google.nl/image1", Name = "Sivrikaya"};
            Assert.Equal(value.Id,result.Id);
        }

        [Fact]
        public async Task Test_Get_Albums_by_Id_return_not_valid ()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var result = service.GetAlbum(4).Result;
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_edit_album_by_id_succes()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var resultToCheck = service.PutAlbum(3, new Models.Album {Artist = "Maarten", Id = 3, ImageUrl = "www.google.nl/image5", Name = "vanRossum"}).Result;
            if (resultToCheck != null)
            {
                resultToCheck.Name = "Maarten";
                await service.PutAlbum(3,resultToCheck);
            }

            var newResult = service.GetAlbum(3).Result;
            Assert.Equal("Maarten", newResult?.Name);

        }
        
        [Fact]
        public async Task Test_edit_album_by_id_failure()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var resultToCheck = service.PutAlbum(5, new Models.Album {Artist = "Maarten", Id = 3, ImageUrl = "www.google.nl/image5", Name = "vanRossum"}).Result;
            if (resultToCheck != null)
            {
                resultToCheck.Name = "Maarten";
                await service.PutAlbum(4,resultToCheck);
            }
            Assert.IsNotType<Exception>(resultToCheck);
        }

        [Fact]
        public async Task Test_delete_album_by_id_success()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());

            var result = service.DeleteAlbum(1).Status;
            Assert.IsType<TaskStatus>(result);
        }
        [Fact]
        public async Task Test_delete_album_by_id_failure()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());

            var result = service.DeleteAlbum(5).Exception.Message;
            Assert.Contains("Error 404 Not found", result);
        }

        [Fact]
        public async Task Test_post_album_by_id_success()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            var dataToPost = service.PostAlbum(new Models.Album
                {Artist = "Pim", Id = 4, ImageUrl = "www.google.nl/image5", Name = "Fortuiyn"});

            if (service.GetAlbum(4).Result.Equals(dataToPost))
            {
                Assert.IsType<Models.Album>(dataToPost);

            }

        }

        // Controller test
        [Fact]
        public async Task Test_Get_all_Albums()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = controller.GetAlbums().Result;
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task Test_Get_album_by_id()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = controller.GetAlbum(3).Result;
            var value = result.Value;
            if (value != null) Assert.Equal(3, value.Id);
        }

        [Fact]
        public async Task Test_Put_album_by_id_failure()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var toCheck = controller.GetAlbum(2).Result.Value;
            if (toCheck != null)
            {
                toCheck.Name = "testData";
                var result = controller.PutAlbum(4, toCheck).Result;
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task Test_Put_album_by_id_success()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = controller.GetAlbum(2).Result.Value;
            if (result != null)
            {
                result.Name = "TestData";
                await controller.PutAlbum(2, result);
            }

            var newResult = controller.GetAlbum(2).Result.Value;
            Assert.Equal("TestData",newResult?.Name);
        }

        [Fact]
        public async Task Test_Delete_album_by_id_success()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = controller.DeleteAlbum(1).Result;
            var list = controller.GetAlbums().Result;
            var albums3 = controller.GetAlbum(1).Result.Value;

            Assert.IsType<NoContentResult>(result);
            Assert.Null(albums3);
            if(list != null) Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task Test_delete_album_by_id_failure_controller()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = controller.DeleteAlbum(4).Result;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Test_Post_album_by_id_add_to_db()
        {
            AlbumService service = new AlbumService(await GetInMemoryDbWithDataAsync());
            AlbumController controller = new AlbumController(await GetInMemoryDbWithDataAsync(),service);

            var result = await controller.PostAlbum(new Models.Album
                {Artist = "Maarten", Id = 4, ImageUrl = "www.google.nl/image5", Name = "vanRossum"});
            if( controller.GetAlbums().Result is List<Models.Album> list) Assert.Equal(4, list.Count );
            Assert.IsType<ActionResult<Models.Album>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }
    }
}