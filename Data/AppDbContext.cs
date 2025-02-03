using Microsoft.EntityFrameworkCore;

namespace Shopping_App___2.Data
{
    public class AppDbContext :DbContext
    {

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.Id);

            modelBuilder.Entity<Product>().ToTable("Products").HasKey(p => p.Id);
            modelBuilder.Entity<Product>().ToTable("Products")
                  .HasOne(p => p.User)
                  .WithMany(u => u.Products)
                  .HasForeignKey(p => p.UserId);


        }
    }
}
