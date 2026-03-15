// Models/ViewModels/RegisterVm.cs
using System.ComponentModel.DataAnnotations;

namespace vlf_4rum.Models.ViewModels   // ← thêm dòng này
{
    public class RegisterVm
    {
        [Required]
        public string Username { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}