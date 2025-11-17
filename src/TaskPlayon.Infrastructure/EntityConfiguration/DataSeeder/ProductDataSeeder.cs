using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskPlayon.Core.Model;

namespace TaskPlayon.Infrastructure.EntityConfiguration.DataSeeder;

public class ProductDataSeeder : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasData(
    new Product
    {
        Id = -1,
        Name = "Laptop",
        Price = 1200.00m,
        Stock = 10,
        CreatedDate = DateTime.Now,
        CreatedBy = 1
    },
    new Product
    {
        Id = -2,
        Name = "Smartphone",
        Price = 800.00m,
        Stock = 20,
        CreatedDate = DateTime.Now,
        CreatedBy = 1
    },
    new Product
    {
        Id = -3,
        Name = "Headphones",
        Price = 150.00m,
        Stock = 50,
        CreatedDate = DateTime.Now,
        CreatedBy = 1
    },
    new Product
    {
        Id = -4,
        Name = "Keyboard",
        Price = 60.00m,
        Stock = 30,
        CreatedDate = DateTime.Now,
        CreatedBy = 1
    },
    new Product
    {
        Id = -5,
        Name = "Mouse",
        Price = 40.00m,
        Stock = 40,
        CreatedDate = DateTime.Now,
        CreatedBy = 1
    }
);

    }
}