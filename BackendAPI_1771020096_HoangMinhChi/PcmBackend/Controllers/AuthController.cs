using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PcmBackend.Data;
using PcmBackend.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PcmBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context; // Để lấy thông tin Member_096

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            // 1. Tìm user theo Email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new { Message = "Email không tồn tại." });

            // 2. Kiểm tra Password
            if (!await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized(new { Message = "Sai mật khẩu." });

            // 3. Lấy Roles (Quyền)
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? "Member";

            // 4. Lấy thông tin chi tiết từ bảng Members_096
            var memberProfile = await _context.Members
                .FirstOrDefaultAsync(m => m.UserId == user.Id);

            // 5. Tạo JWT Token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id), // Quan trọng: Để xác định user khi gọi API khác
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3), // Token sống 3 tiếng
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 6. Trả về kết quả cho Mobile App
            return Ok(new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Email = user.Email,
                Role = role,
                // Nếu là Admin thì có thể không có MemberProfile, cần null check
                FullName = memberProfile?.FullName ?? "Administrator",
                AvatarUrl = memberProfile?.AvatarUrl ?? "",
                WalletBalance = memberProfile?.WalletBalance ?? 0,
                Tier = memberProfile?.Tier.ToString() ?? "Standard"
            });
        }
    }
}