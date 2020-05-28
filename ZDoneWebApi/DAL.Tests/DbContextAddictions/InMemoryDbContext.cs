using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ZDoneWebApi.Data;
using ZDoneWebApi.Data.Models;

namespace DAL.Tests.DbContextAddictions
{
    internal class InMemoryDbContext
    {
        private static InMemoryDbContext _instance = new InMemoryDbContext();

        public AppDbContext GetContextWithData()
        {
            return InsertDataToDB(GetEmptyContextInMemory());
        }

        public AppDbContext GetEmptyContextInMemory()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DbInMemory")
                .Options;

            return new AppDbContext(options);
        }

        internal AppDbContext InsertDataToDB(AppDbContext context)
        {
            Project project1 = new Project { Id = 1, Name = "1 Project" };
            Project project2 = new Project { Id = 2, Name = "2 Project" };

            Item item1 = new Item { Id = 1, Name = "1 item", StatusId = 1, ListId = 1, IsDone = false, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "1 description", ParentId = null, Priority = null };
            Item item2 = new Item { Id = 2, Name = "2 item", StatusId = 2, ListId = 1, IsDone = false, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "2 description", ParentId = null, Priority = null };
            Item item3 = new Item { Id = 3, Name = "3 item", StatusId = 1, ListId = 2, IsDone = true, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "3 description", ParentId = 1, Priority = 1 };
            Item item4 = new Item { Id = 4, Name = "4 item", StatusId = 2, ListId = 2, IsDone = false, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "4 description", ParentId = null, Priority = null };
            Item item5 = new Item { Id = 5, Name = "5 item", StatusId = 3, ListId = 1, IsDone = false, ExpiredDate = DateTime.Now.AddDays(7), CreatedDate = DateTime.Now, Description = "5 description", ParentId = null, Priority = null };

            List list1 = new List { Id = 1, FolderId = 1, Name = "1 List" };
            List list2 = new List { Id = 2, FolderId = 2, Name = "2 List" };
            List list3 = new List { Id = 3, FolderId = 1, Name = "3 List" };

            Folder folder1 = new Folder { Id = 1, Name = "1 Folder", ProjectId = 1 };
            Folder folder2 = new Folder { Id = 2, Name = "2 Folder", ProjectId = 1 };
            Folder folder3 = new Folder { Id = 3, Name = "3 Folder", ProjectId = 1 };

            Status status1 = new Status { Id = 1, Name = "New" };
            Status status2 = new Status { Id = 2, Name = "Active" };
            Status status3 = new Status { Id = 3, Name = "Done" };

            Comment comment1 = new Comment { Id = 1, ItemId = 1, Text = "1 comment", UserId = "1", Date = DateTime.Today };
            Comment comment2 = new Comment { Id = 2, ItemId = 2, Text = "2 comment", UserId = "2", Date = DateTime.Today };

            // User user1 = new User {  };
            //User user2 = new User { };

            ProjectsUsers projectsUsers1 = new ProjectsUsers { ProjectId = 1, UserId = "1" };
            ProjectsUsers projectsUsers2 = new ProjectsUsers { ProjectId = 2, UserId = "2" };

            context.Comments.AddRange(new[] { comment1, comment2 });
            context.Projects.AddRange(new[] { project1, project2 });
            context.Items.AddRange(new[] { item1, item2, item3, item4, item5 });
            context.Statuses.AddRange(new[] { status1, status2, status3 });
            //context.Users.AddRange(new[] { user1, user2 });
            context.ProjectsUsers.AddRange(new[] { projectsUsers1, projectsUsers2 });
            context.Folders.AddRange(new[] { folder1, folder2, folder3 });
            context.Lists.AddRange(new[] { list1, list2, list3 });

            context.SaveChanges();
            return context;
        }
    }
}