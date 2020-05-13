using DAL.Tests.DbContextAddictions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ZDoneWebApi.Data.Models;
using ZDoneWebApi.Repositories;
using ZDoneWebApi.Repositories.Interfaces;

namespace DAL.Tests
{
    public class FolderRepositoryTest
    {
        [Fact]
        public async Task ReadAllAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IFolderRepository repository = new FolderRepository(context);
                //Act
                List<Folder> expected = context.Folders.ToList();
                IEnumerable<Folder> actual = await repository.ReadAll();
                //Assert
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.ToList().Count);
                Assert.Equal(expected.Count, context.Folders.ToList().Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task ReadAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IFolderRepository repository = new FolderRepository(context);
                //Act
                Folder expected = context.Folders.Find(1);
                var actual = await repository.Read(expected.Id);
                //Assert
                Assert.Equal(expected.Id, context.Folders.FirstOrDefault(r => r.Id == 1).Id);
                Assert.Equal(expected.Id, actual.Id);
                Assert.IsAssignableFrom<Folder>(actual);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task ReadAsyncNotExist()
        {
            //Arrange
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IFolderRepository repository = new FolderRepository(context);
                //Act
                Folder expected = context.Folders.Find(1);
                Folder actual = await repository.Read(-1);
                //Assert

                Assert.Null(actual);
                Assert.Null(context.Folders.FirstOrDefault(r => r.Id == -1));
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task CreateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetEmptyContextInMemory())
            {
                IFolderRepository repository = new FolderRepository(context);

                Folder folder = new Folder { Id = 3, Name = "Name 3", ProjectId = 1 };
                await repository.Create(folder);

                var actual = context.Folders.Find(3);

                Assert.NotNull(actual);
                Assert.Equal(folder.Id, actual.Id);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UpdateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IFolderRepository repo = new FolderRepository(context);
                Folder item2 = context.Folders.FirstOrDefault();
                string oldName = item2.Name;
                item2.Name += "1111";
                await repo.Update(item2);
                Folder updatedItem = context.Folders.Find(item2.Id);

                Assert.NotEqual(updatedItem.Name, oldName);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task DeleteAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            Folder folder = new Folder { Id = 4, Name = "Name 4", ProjectId = 1 };
            using (var context = cls.GetEmptyContextInMemory())
            {
                IFolderRepository repo = new FolderRepository(context);
                context.Folders.Add(folder);
                context.SaveChanges();
                var added = context.Folders.Find(folder.Id);

                await repo.Delete(folder.Id);
                var searched = await repo.Read(folder.Id);
                //Assert
                Assert.NotNull(added);
                Assert.Null(searched);
                context.Database.EnsureDeleted();
            }
        }
    }
}