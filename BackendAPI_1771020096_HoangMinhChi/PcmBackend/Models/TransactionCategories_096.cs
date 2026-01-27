using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PcmBackend.Models
{
    [Table("096_TransactionCategories")]
    public class TransactionCategories_096
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty; // Ví dụ: "Thu", "Chi"
    }
}