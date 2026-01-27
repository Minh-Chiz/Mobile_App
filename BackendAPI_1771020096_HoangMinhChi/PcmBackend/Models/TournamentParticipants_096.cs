using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_TournamentParticipants")]
    public class TournamentParticipants_096
    {
        [Key]
        public int Id { get; set; }

        public int TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public Tournaments_096? Tournament { get; set; }

        public int MemberId { get; set; }
        // [ForeignKey("MemberId")] public Members_096? Member { get; set; } // Uncomment khi có class Member

        public string? TeamName { get; set; } // Tên đội nếu đánh đôi

        public bool PaymentStatus { get; set; } // Đã đóng tiền hay chưa
    }
}