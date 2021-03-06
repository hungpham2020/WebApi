using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Model
{
    public class ApplicationDbContext: IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }
        public DbSet<TodoItems> TodoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
        }
        private void SeedUsers(ModelBuilder builder)
        {
            var passwordHasher = new PasswordHasher<CustomUser>();
            CustomUser user = new CustomUser()
            {
                Id = "1",
                UserName = "Admin",
                Email = "admin@gmail.com",
                NormalizedUserName = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
                LockoutEnabled = true,
                EmailConfirmed = true,
            };
            CustomUser user2 = new CustomUser()
            {
                Id = "2",
                UserName = "Staff",
                Email = "staff@gmail.com",
                NormalizedUserName = "staff",
                PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
                LockoutEnabled = true,
                EmailConfirmed = true,
            };


            builder.Entity<CustomUser>().HasData(user);
            builder.Entity<CustomUser>().HasData(user2);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "1", Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "admin" },
                new IdentityRole() { Id = "2", Name = "Staff", ConcurrencyStamp = "2", NormalizedName = "staff" }
                );
        }
        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "1", UserId = "1" });
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "2", UserId = "2" });
        }
    }
}
