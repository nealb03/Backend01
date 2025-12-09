using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApi.Models
{
    [Table("Transactions")] // ✅ maps to your Transactions table in MySQL
    public partial class Transaction
    {
        [Key]
        [Column("TransactionId")]
        public int TransactionId { get; set; }

        [Column("AccountId")]
        public int AccountId { get; set; }

        [Column("Amount", TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column("Type")]
        [StringLength(50)]
        public string Type { get; set; } = null!;

        [Column("Description")]
        [StringLength(255)]
        public string Description { get; set; } = null!;

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        // ✅ Correct navigation property
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }
    }
}