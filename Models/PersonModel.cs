using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EventBooking.Models;

#nullable disable

namespace EventBooking.Models
{
    [Table("person")]
    public class Person
    {
        [Key]
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // "id, primary key"

        // Foreign key to event
        [ForeignKey("Event")]
        [Column("event_id")]
        [Required]
        public int EventId { get; set; }

        public virtual Event Event { get; set; }

        [Column("first_name", TypeName = "text")]
        [Required]
        public string FirstName { get; set; }

        [Column("last_name", TypeName = "text")]
        [Required]
        public string LastName { get; set; }

        [Column("identification_number", TypeName = "text")]
        [Required]
        public string IdentificationNumber { get; set; }

        [Column("payment_method", TypeName = "text")]
        [Required]
        [RegularExpression("^(Cash|BankTransfer)$", ErrorMessage = "Invalid payment method. Valid methods are 'Cash' and 'BankTransfer'.")]
        public string PaymentMethod { get; set; }

        [Column("info", TypeName = "varchar(5000)")]
        [Required]
        [MaxLength(1000)]
        public string Info { get; set; } // "Additional information"
    }
}
