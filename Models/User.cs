using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApi.Models
{
    [Table("Users")]
    public partial class User
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Column("FirstName")]
        [StringLength(100)]
        public string? FirstName { get; set; }

        [Column("LastName")]
        [StringLength(100)]
        public string? LastName { get; set; }

        [Column("Email")]
        [StringLength(255)]
        public string? Email { get; set; }

        // password column exactly as it exists in MySQL
        [Column("Password")]
        [StringLength(255)]
        public string? Password { get; set; }

        // ❌  remove or comment out this line if the table has no CreatedAt column
        // [Column("CreatedAt")]
        // public DateTime? CreatedAt { get; set; }

        // Navigation collection
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}