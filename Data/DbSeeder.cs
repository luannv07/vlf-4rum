using System;
using System.Linq;
using BCrypt.Net;
using vlf_4rum.Data;
using Vlf4rum.Models;

namespace Vlf4rum.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var admin = new User
                {
                    Username = "luan",
                    Email = "admin@vlf4rum.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("luan"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(admin);
                context.SaveChanges();
            }
        }
    }
}