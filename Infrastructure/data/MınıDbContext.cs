using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Infrastructure.data
{
    public class MınıDbContext : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        public MınıDbContext(DbContextOptions<MınıDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Başlangıç verilerini ekleyin
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Electronics" },
                new Category { CategoryId = 2, Name = "Books" }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Laptop", Price = 999.99M, CategoryId = 1,ImgUrl= "https://picsum.photos/200" },
                new Product { ProductId = 2, ProductName = "Smartphone", Price = 699.99M, CategoryId = 1, ImgUrl= "https://picsum.photos/200" },
                new Product { ProductId = 3, ProductName = "C# Programming Book", Price = 29.99M, CategoryId = 2, ImgUrl = "https://picsum.photos/200" }
            );
        }


    }
}
