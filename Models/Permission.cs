// Permission.cs

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Vlf4rum.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Permission : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        // Navigation
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    }
}