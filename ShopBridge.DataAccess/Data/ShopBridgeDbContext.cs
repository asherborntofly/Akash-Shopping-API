using Microsoft.EntityFrameworkCore;
using ShopBridge.DTO;

namespace ShopBridge.DataAccess
{
    public class ShopBridgeDbContext : DbContext
    {
        public ShopBridgeDbContext(DbContextOptions<ShopBridgeDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the database schema
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("ProductId").ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Quantity);
            });
        }

        public void Seed()
        {
            Products.Add(new Product
            {
                Name = "Product 1",
                Description = "This is product 1",
                Price = 10.99m,
                Quantity = 50
            });
            Products.Add(new Product
            {
                Name = "Product 2",
                Description = "This is product 2",
                Price = 20.99m,
                Quantity = 100
            });
            Products.Add(new Product
            {
                Name = "Product 3",
                Description = "This is product 3",
                Price = 30.99m,
                Quantity = 200
            });
            Products.Add(new Product
            {
                Name = "Product 4",
                Description = "This is product 4",
                Price = 40.99m,
                Quantity = 300
            });
            SaveChanges();
        }
    }
}
