using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Models
{
    [Table("event")]
    public class Event
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // "Event id, primary key"

        [Column("name", TypeName = "text")]
        [Required]
        public string Name { get; set; } // "Event name"

        [Column("timestamp", TypeName = "timestamp with time zone")]
        [Required]
        public DateTime Timestamp { get; set; } // "Event timestamp"

        [Column("location", TypeName = "text")]
        [Required]
        public string Location { get; set; } // "Event location string"

        [Column("info", TypeName = "varchar(1000)")]
        [Required]
        [MaxLength(1000)]
        public string Info { get; set; } // "Additional information"
    }

    // Another model to show number of participants for an event
    public class EventWithParticipants : Event
    {
        [Column("participants")]
        public int Participants { get; set; }
    }
}
