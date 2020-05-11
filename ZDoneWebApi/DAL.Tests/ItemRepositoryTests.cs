using DAL.Tests.DbContextAddictions;
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
    public class ItemRepositoryTests
    {
        [Fact]
        public async Task ReadAllAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IItemRepository repository = new ItemRepository(context);
                //Act
                List<Item> expected = context.Items.ToList();
                IEnumerable<Item> actual = await repository.ReadAll();
                //Assert
                Assert.True(actual != null);
                Assert.Equal(expected.Count, actual.ToList().Count);
                Assert.Equal(expected.Count, context.Items.ToList().Count);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task ReadAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IItemRepository repository = new ItemRepository(context);
                //Act
                Item expected = context.Items.Find(1);
                var actual = await repository.Read(expected.Id);
                //Assert
                Assert.Equal(expected.Id, context.Items.FirstOrDefault(r => r.Id == 1).Id);
                Assert.Equal(expected.Id, actual.Id);
                Assert.IsAssignableFrom<Item>(actual);
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
                IItemRepository repository = new ItemRepository(context);
                //Act
                Item expected = context.Items.Find(1);
                Item actual = await repository.Read(-1);
                //Assert

                Assert.Null(actual);
                Assert.Null(context.Items.FirstOrDefault(r => r.Id == -1));
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task CreateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetEmptyContextInMemory())
            {
                IItemRepository repository = new ItemRepository(context);

                Item item = new Item { Id = 3, Name = "3 item", StatusId = 1, ListId = 2, IsDone = true, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "3 description", ParentId = 1, Priority = 1 };
                await repository.Create(item);

                var actual = context.Items.Find(3);

                Assert.NotNull(actual);
                Assert.Equal(item.Id, actual.Id);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task UpdateAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            using (var context = cls.GetContextWithData())
            {
                IItemRepository repo = new ItemRepository(context);
                Item item2 = context.Items.FirstOrDefault();
                string oldName = item2.Name;
                item2.Name += "1111";
                await repo.Update(item2);
                Item updatedItem = context.Items.Find(item2.Id);

                Assert.NotEqual(updatedItem.Name, oldName);
                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async Task DeleteAsyncDefault()
        {
            var cls = new InMemoryDbContext();
            Item item1 = new Item { Id = 3, Name = "3 item", StatusId = 1, ListId = 2, IsDone = true, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "3 description", ParentId = 1, Priority = 1 };
            using (var context = cls.GetEmptyContextInMemory())
            {
                IItemRepository repo = new ItemRepository(context);
                context.Items.Add(item1);
                context.SaveChanges();
                var added = context.Items.Find(item1.Id);

                await repo.Delete(item1.Id);
                var searched = await repo.Read(item1.Id);
                //Assert
                Assert.NotNull(added);
                Assert.Null(searched);
                context.Database.EnsureDeleted();
            }
        }
    }
}