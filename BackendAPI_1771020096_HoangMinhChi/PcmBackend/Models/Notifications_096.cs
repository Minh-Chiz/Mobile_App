using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_Notifications")]
    public class Notifications_096
    {
        [Key]
        public int Id { get; set; }

        public int ReceiverId { get; set; }
        [ForeignKey("ReceiverId")]
        public Members_096? Receiver { get; set; }

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public string? LinkUrl { get; set; }

        public bool IsRead { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}