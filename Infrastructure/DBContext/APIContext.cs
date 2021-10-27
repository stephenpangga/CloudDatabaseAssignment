using System;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure
{
    public class APIContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        public APIContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "",
                "",
                databaseName: ""
                );
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("WidgetAndCO");

            //orderinformation
            modelBuilder.Entity<Order>()
                .ToContainer(nameof(Orders))
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .HasPartitionKey(o => o.PartitionKey);


            modelBuilder.Entity<User>()
                .ToContainer(nameof(Users))
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.PartitionKey);

        }
    }
}
