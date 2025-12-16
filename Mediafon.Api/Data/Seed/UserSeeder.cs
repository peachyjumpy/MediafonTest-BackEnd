using Mediafon.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace Mediafon.Api.Data.Seed
{
    public static class UserSeeder
    {
        public static void SeedUsers(MediafonDbContext context)
        {
            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<User>();

                var user1 = new User
                {
                    Username = "luis"
                };
                user1.PasswordHash = passwordHasher.HashPassword(user1, "12345");

                var user2 = new User
                {
                    Username = "user2"
                };
                user2.PasswordHash = passwordHasher.HashPassword(user2, "User2@123");

                var user3 = new User
                {
                    Username = "user3"
                };
                user3.PasswordHash = passwordHasher.HashPassword(user3, "User3@123");

                context.Users.Add(user1);
                context.Users.Add(user2);
                context.Users.Add(user3);
                context.SaveChanges();
            }
        }
    }
}
