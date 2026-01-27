using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_Members")]
    public class Members_096
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; } = DateTime.Now;

        public double RankLevel { get; set; }

        public bool IsActive { get; set; } // Status

        // Liên kết Identity User
        public string? UserId { get; set; }
        // public AppUser? User {get;set;} // Nếu bạn dùng IdentityUser tùy chỉnh

        // Advanced Fields
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletBalance { get; set; }

        public RankLevel Tier { get; set; } // Enum

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSpent { get; set; }

        public string? AvatarUrl { get; set; }
    }
}