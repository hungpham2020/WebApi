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

        }
        private void SeedUsers(ModelBuilder builder)
        {
            CustomUser user = new CustomUser()
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                LockoutEnabled = false,
                EmailConfirmed = true,
            };
            PasswordHasher<CustomUser> passwordHasher = new PasswordHasher<CustomUser>();
        }
    }
}
