using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using vlf_4rum.Data;
using Vlf4rum.Constants;
using Vlf4rum.Models;

namespace Vlf4rum.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!await db.Roles.AnyAsync())
                await SeedRolesAsync(db);

            if (!await db.Permissions.AnyAsync())
                await SeedPermissionsAsync(db);

            if (!await db.RolePermissions.AnyAsync())
                await SeedRolePermissionsAsync(db);

            if (!await db.Users.AnyAsync())
                await SeedAdminUserAsync(db);
        }

        // ── Roles ───────────────────────────────────────────
        private static async Task SeedRolesAsync(AppDbContext db)
        {
            var defined = new List<(string Name, string Desc)>
            {
                (AppRoles.Admin,     "Quản trị viên"),
                (AppRoles.Moderator, "Kiểm duyệt viên"),
                (AppRoles.Member,    "Thành viên"),
            };

            foreach (var (name, desc) in defined)
            {
                if (!await db.Roles.AnyAsync(r => r.Name == name))
                    db.Roles.Add(new Role { Name = name, Description = desc });
            }

            await db.SaveChangesAsync();
        }

        // ── Permissions ─────────────────────────────────────
        private static async Task SeedPermissionsAsync(AppDbContext db)
        {
            var defined = typeof(AppPermissions)
                .GetNestedTypes()
                .SelectMany(t => t.GetFields())
                .Select(f => f.GetValue(null)!.ToString()!)
                .ToList();

            foreach (var name in defined)
            {
                if (!await db.Permissions.AnyAsync(p => p.Name == name))
                    db.Permissions.Add(new Permission { Name = name });
            }

            await db.SaveChangesAsync();
        }

        // ── RolePermissions ─────────────────────────────────
        private static async Task SeedRolePermissionsAsync(AppDbContext db)
        {
            var adminRole = await db.Roles.FirstAsync(r => r.Name == AppRoles.Admin);
            var modRole = await db.Roles.FirstAsync(r => r.Name == AppRoles.Moderator);
            var memberRole = await db.Roles.FirstAsync(r => r.Name == AppRoles.Member);

            var allPerms = await db.Permissions.ToListAsync();

            // Admin = tất cả permission
            await GrantAsync(db, adminRole.Id, allPerms.Select(p => p.Name).ToArray(), allPerms);

            // Moderator
            await GrantAsync(db, modRole.Id, new[]
            {
                AppPermissions.Thread.View,
                AppPermissions.Thread.Create,
                AppPermissions.Thread.Edit,
                AppPermissions.Thread.EditAny,
                AppPermissions.Thread.Delete,
                AppPermissions.Thread.DeleteAny,
                AppPermissions.Thread.Pin,
                AppPermissions.Thread.Lock,
                AppPermissions.Thread.Move,

                AppPermissions.Reply.View,
                AppPermissions.Reply.Create,
                AppPermissions.Reply.Edit,
                AppPermissions.Reply.EditAny,
                AppPermissions.Reply.Delete,
                AppPermissions.Reply.DeleteAny,

                AppPermissions.Category.View,

                AppPermissions.User.ViewProfile,
                AppPermissions.User.EditProfile,
                AppPermissions.User.Ban,
                AppPermissions.User.Unban,
                AppPermissions.User.ViewList,

                AppPermissions.Report.Create,
                AppPermissions.Report.View,
                AppPermissions.Report.Resolve,

                AppPermissions.Reaction.Create,
                AppPermissions.Reaction.Delete,

                AppPermissions.Tag.View,
                AppPermissions.Tag.Assign,

                AppPermissions.Search.Basic,
                AppPermissions.Search.Advanced,

                AppPermissions.Notification.Receive,
            }, allPerms);

            // Member
            await GrantAsync(db, memberRole.Id, new[]
            {
                AppPermissions.Thread.View,
                AppPermissions.Thread.Create,
                AppPermissions.Thread.Edit,
                AppPermissions.Thread.Delete,

                AppPermissions.Reply.View,
                AppPermissions.Reply.Create,
                AppPermissions.Reply.Edit,
                AppPermissions.Reply.Delete,

                AppPermissions.Category.View,

                AppPermissions.User.ViewProfile,
                AppPermissions.User.EditProfile,

                AppPermissions.Report.Create,

                AppPermissions.Reaction.Create,
                AppPermissions.Reaction.Delete,

                AppPermissions.Tag.View,

                AppPermissions.Search.Basic,

                AppPermissions.Notification.Receive,
            }, allPerms);

            await db.SaveChangesAsync();
        }

        // ── Admin user ──────────────────────────────────────
        private static async Task SeedAdminUserAsync(AppDbContext db)
        {
            if (!await db.Users.AnyAsync())
            {
                var adminRole = await db.Roles.FirstAsync(r => r.Name == AppRoles.Admin);

                var admin = new User
                {
                    Username = "luan",
                    Email = "admin@vlf4rum.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("luan"),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                db.Users.Add(admin);
                await db.SaveChangesAsync();

                db.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleId = adminRole.Id
                });

                await db.SaveChangesAsync();
            }
        }

        // ── Helper ──────────────────────────────────────────
        private static async Task GrantAsync(
            AppDbContext db,
            int roleId,
            string[] permissionNames,
            List<Permission> allPerms)
        {
            foreach (var name in permissionNames)
            {
                var perm = allPerms.FirstOrDefault(p => p.Name == name);
                if (perm == null) continue;

                if (!await db.RolePermissions.AnyAsync(rp =>
                        rp.RoleId == roleId && rp.PermissionId == perm.Id))
                {
                    db.RolePermissions.Add(new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = perm.Id,
                    });
                }
            }
        }
    }
}
