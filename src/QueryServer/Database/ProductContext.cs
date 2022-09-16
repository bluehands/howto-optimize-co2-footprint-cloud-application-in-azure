using Application;
using FunicularSwitch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database;

public class ProductContext : DbContext
{
    public DbSet<Product> Products { get; set; }


    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.HasKey(product => product.ProductId);
            builder
                .Property(product => product.EuroPrice)
                .HasPrecision(10);
            builder
                .Property(product => product.ImageSource)
                .IsRequired(false)
                .HasConversion(
                    e => e.GetValueOrDefault(),
                    s => s.ToOption(),
                    new ValueComparer<Option<string>>(
                        (c1, c2) => c1 != null && c2 != null && c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.Convert<string>()));
            
            builder.HasData(Enumerable.Range(0, 1000).Select(e => Fake.FakeProduct()));
        });
    }
}