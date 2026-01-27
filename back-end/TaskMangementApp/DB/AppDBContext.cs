using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaskMangementApp.Models;

namespace TaskMangementApp.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=taskmanagement.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects");
            modelBuilder.Entity<Models.Task>().ToTable("Tasks");
            modelBuilder.Entity<TaskLog>().ToTable("TaskLogs");
            modelBuilder.Entity<User>().ToTable("Users");
        }

    }
}
