// UserRole.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Vlf4rum.Models
{
    [Index(nameof(UserId), IsUnique = true)] // 1 user chỉ có 1 role
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}