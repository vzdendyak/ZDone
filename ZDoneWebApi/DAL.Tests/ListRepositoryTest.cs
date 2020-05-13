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
    public class ListRepositoryTest
    {
        [Fact]
        public async Task ReadAllAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IListRepository repository = new ListRepository(context);
                //Act
                List<List> expected = context.Lists.ToList();
                IEnumerable<List> actual = await repository.ReadAll();
                //Assert
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.ToList().Count);
                Assert.Equal(expected.Count, context.Lists.ToList().Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task ReadAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IListRepository repository = new ListRepository(context);
                //Act
                List expected = context.Lists.Find(1);
                var actual = await repository.Read(expected.Id);
                //Assert
                Assert.Equal(expected.Id, context.Lists.FirstOrDefault(r => r.Id == 1).Id);
                Assert.Equal(expected.Id, actual.Id);
                Assert.IsAssignableFrom<List>(actual);
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
                IListRepository repository = new ListRepository(context);
                //Act
                List expected = context.Lists.Find(1);
                List actual = await repository.Read(-1);
                //Assert

                Assert.Null(actual);
                Assert.Null(context.Lists.FirstOrDefault(r => r.Id == -1));
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task CreateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetEmptyContextInMemory())
            {
                IListRepository repository = new ListRepository(context);

                List list = new List { Id = 3, Name = "List 3", FolderId = 1 };
                await repository.Create(list);

                var actual = context.Lists.Find(3);

                Assert.NotNull(actual);
                Assert.Equal(list.Id, actual.Id);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UpdateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IListRepository repo = new ListRepository(context);
                List item2 = context.Lists.FirstOrDefault();
                string oldName = item2.Name;
                item2.Name += "1111";
                await repo.Update(item2);
                List updatedItem = context.Lists.Find(item2.Id);

                Assert.NotEqual(updatedItem.Name, oldName);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task DeleteAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            List list = new List { Id = 4, Name = "List 4", FolderId = 1 };
            using (var context = cls.GetEmptyContextInMemory())
            {
                IListRepository repo = new ListRepository(context);
                context.Lists.Add(list);
                context.SaveChanges();
                var added = context.Lists.Find(list.Id);

                await repo.Delete(list.Id);
                var searched = await repo.Read(list.Id);
                //Assert
                Assert.NotNull(added);
                Assert.Null(searched);
                context.Database.EnsureDeleted();
            }
        }
    }
}