using Microsoft.EntityFrameworkCore;
using ZDoneWebApi.Data.Models;

namespace ZDoneWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProjectsUsers> ProjectsUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Status)
                .WithMany(s => s.Items)
                .HasForeignKey(i => i.StatusId);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.List)
                .WithMany(list => list.Items)
                .HasForeignKey(item => item.ListId);

            modelBuilder.Entity<List>()
                .HasOne(list => list.Folder)
                .WithMany(folder => folder.Lists)
                .HasForeignKey(list => list.FolderId);

            modelBuilder.Entity<Folder>()
                .HasOne(f => f.Project)
                .WithMany(project => project.Folders)
                .HasForeignKey(folder => folder.ProjectId);

            modelBuilder.Entity<Project>()
                .HasKey(p => new { p.Id });

            modelBuilder.Entity<ProjectsUsers>()
                .HasKey(pu => new { pu.UserId, pu.ProjectId });

            modelBuilder.Entity<ProjectsUsers>()
                .HasOne(pu => pu.Project)
                .WithMany(p => p.ProjectsUsers)
                .HasForeignKey(pu => pu.ProjectId);

            modelBuilder.Entity<ProjectsUsers>()
                .HasOne(pu => pu.User)
                .WithMany(u => u.ProjectsUsers)
                .HasForeignKey(pu => pu.UserId);

            modelBuilder.Entity<Item>().HasOne(i => i.Parent).WithMany(ii => ii.Children).HasForeignKey(i => i.ParentId);
        }
    }
}