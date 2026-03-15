// UserPermission.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Vlf4rum.Models
{
    [Index(nameof(UserId), nameof(PermissionId), IsUnique = true)]
    public class UserPermission : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;

        public bool IsGranted { get; set; } = true;
    }
}