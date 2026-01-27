using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_Matches")]
    public class Matches_096
    {
        [Key]
        public int Id { get; set; }

        public int? TournamentId { get; set; }
        [ForeignKey("TournamentId")]
        public Tournaments_096? Tournament { get; set; }

        public string? RoundName { get; set; } // VD: "Group A", "Quarter Final"

        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }

        // Participants
        public int Team1_Player1Id { get; set; }
        public int? Team1_Player2Id { get; set; } // Nullable nếu đánh đơn

        public int Team2_Player1Id { get; set; }
        public int? Team2_Player2Id { get; set; }

        // Result
        public int Score1 { get; set; }
        public int Score2 { get; set; }

        public string? Details { get; set; } // VD: "11-9, 5-11, 11-8"

        public MatchWinningSide? WinningSide { get; set; }

        public bool IsRanked { get; set; } // Có tính điểm DUPR không

        public MatchStatus Status { get; set; }
    }
}