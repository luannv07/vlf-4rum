// RolePermission.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Vlf4rum.Models
{
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true)]
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}