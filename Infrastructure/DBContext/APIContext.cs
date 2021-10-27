using System;
using Microsoft.EntityFrameworkCore;
using Domain;

namespace Infrastructure
{
    public class APIContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public APIContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("WidgetAndCO");

            //order
            modelBuilder.Entity<Order>()
                .ToContainer(nameof(Orders))
                .HasKey(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .HasNoDiscriminator()
                .UseETagConcurrency()
                .HasPartitionKey(o => o.PartitionKey);

            //user
            modelBuilder.Entity<User>()
                .ToContainer(nameof(Users))
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasNoDiscriminator()
                .UseETagConcurrency()
                .HasPartitionKey(u => u.PartitionKey);

            //product
            modelBuilder.Entity<Product>()
                .ToContainer(nameof(Product))
                .HasKey(p => p.ProductId);

            modelBuilder.Entity<Product>()
                .HasNoDiscriminator()
                .UseETagConcurrency()
                .HasPartitionKey(p => p.PartitionKey);


        }
    }
}
