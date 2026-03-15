using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VlfForum.Controllers;
using VlfForum.Services.Interfaces;
using vlf_4rum.Data;

namespace vlf_4rum.Controllers
{
    public class UserController : BaseController
    {
        private readonly AppDbContext _db;

        public UserController(ICurrentUserService currentUser, AppDbContext db) : base(currentUser)
        {
            _db = db;
        }

        [HttpGet]
        [Route("u/{username}")]
        public async Task<IActionResult> Profile(string username)
        {
            var user = await _db.Users
                .Include(u => u.UserRole)
                    .ThenInclude(ur => ur!.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        [Authorize]
        [Route("u/edit")]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _db.Users.FindAsync(_currentUser.UserId);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [Authorize]
        [Route("u/edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(string? fullName, string? bio, string? avatarLink)
        {
            var user = await _db.Users.FindAsync(_currentUser.UserId);
            if (user == null) return NotFound();

            user.FullName = fullName;
            user.Bio = bio;
            user.AvatarLink = avatarLink;
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            TempData["Toast.Variant"] = "success";
            TempData["Toast.Message"] = "Cập nhật profile thành công!";
            return RedirectToAction("Profile", new { username = user.Username });
        }
    }
}