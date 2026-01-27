using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_Bookings")]
    public class Bookings_096
    {
        [Key]
        public int Id { get; set; }

        public int CourtId { get; set; }
        [ForeignKey("CourtId")]
        public Courts_096? Court { get; set; }

        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Members_096? Member { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public int? TransactionId { get; set; }
        [ForeignKey("TransactionId")]
        public WalletTransactions_096? Transaction { get; set; }

        // Advanced
        public bool IsRecurring { get; set; }
        public string? RecurrenceRule { get; set; }
        public int? ParentBookingId { get; set; }

        public BookingStatus Status { get; set; }

        // [Mới] Thêm trường này để check dọn dẹp booking quá hạn
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}