using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApi.Models
{
    [Table("Accounts")]
    public partial class Account
    {
        [Key]
        [Column("AccountId")]
        public int AccountId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("AccountNumber")]
        [StringLength(50)]
        public string? AccountNumber { get; set; }   // ✅ add this property if the column exists

        [Column("AccountType")]
        [StringLength(100)]
        public string? AccountType { get; set; }

        [Column("Balance", TypeName = "decimal(18,2)")]
        public decimal? Balance { get; set; }

        [Column("CreatedAt")]
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}