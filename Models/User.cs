using System;
using System.ComponentModel.DataAnnotations;

namespace Vlf4rum.Models
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = null!;

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = null!;

        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? AvatarLink { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? LastLoginAt { get; set; }
    }
}