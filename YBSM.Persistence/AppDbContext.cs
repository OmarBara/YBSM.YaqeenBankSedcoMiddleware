using Core.Domain.Entities;
using Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRM.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthSession> AuthSessions { get; set; }
      /*  public DbSet<Category> Categories { get; set; }
        public DbSet<StoreInfo> StoreInformation { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<ImageStore> ImageStores { get; set; }
        public DbSet<Activity> Activities { get; set; }*/

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* modelBuilder.ApplyConfiguration(new MerchantConfig());
             modelBuilder.ApplyConfiguration(new CategoryConfig());
             modelBuilder.ApplyConfiguration(new StoreConfig());
            */
            var passwordHasher = new PasswordHasher<dynamic>();
            var hasher = passwordHasher.HashPassword(null, "123456");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Email = "ahmed@email.com",
                    Password = hasher,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = "0926032402",
                    UserRole = Roles.Admin,
                    State = State.Active,
                    Id = Guid.NewGuid(),
                }
            );


            base.OnModelCreating(modelBuilder);
        }
    }
}