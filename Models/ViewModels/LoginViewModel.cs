using System.ComponentModel.DataAnnotations;

namespace vlf_4rum.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = "";

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}