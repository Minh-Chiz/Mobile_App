using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_Tournaments")]
    public class Tournaments_096
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TournamentFormat Format { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal EntryFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrizePool { get; set; }

        public TournamentStatus Status { get; set; }

        // Settings lưu dưới dạng JSON string
        public string? Settings { get; set; }

        // Navigation Properties
        public ICollection<TournamentParticipants_096>? Participants { get; set; }
        public ICollection<Matches_096>? Matches { get; set; }
    }
}