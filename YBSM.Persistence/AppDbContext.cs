using Core.Domain.Entities;
using Core.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YBSM.Core.Domain.Entities;

namespace HRM.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<AuthSession> AuthSessions { get; set; }      
        public DbSet<LypayTransaction> LypayTransaction { get; set; }


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
                    Email = "bara@email.com",
                    Password = hasher,
                    CreatedDate = DateTime.UtcNow,
                    PhoneNumber = "0926032402",
                    UserRole = Roles.Admin,
                    State = State.Active,
                    Id = Guid.NewGuid(),
                }
            );
            modelBuilder.Entity<LypayTransaction>().HasNoKey();
            // modelBuilder.Entity<LypayTransaction>().OwnsOne(x => x.F1);

            base.OnModelCreating(modelBuilder);
        }
    }
}