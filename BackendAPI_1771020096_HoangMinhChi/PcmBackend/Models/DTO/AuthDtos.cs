using System.ComponentModel.DataAnnotations;

namespace PcmBackend.Models.DTO
{
    // Dữ liệu client gửi lên để đăng nhập
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    // Dữ liệu server trả về cho App Mobile
    public class LoginResponseDto
    {
        public string Token { get; set; }       // JWT Token để lưu vào storage
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }        // Admin/Member
        public string AvatarUrl { get; set; }
        public decimal WalletBalance { get; set; }
        public string Tier { get; set; }        // Hạng thành viên
    }
}