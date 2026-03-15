// Role.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Vlf4rum.Models
{
    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        public string? Description { get; set; }

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}