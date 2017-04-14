
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelPlanner.DB
{
    [Table("Trips")]
    public class Trip
    {
        [Column("Id"), Key]
        public long Id { get; set; }

        [Column("UserId"), Required]
        public long UserId { get; set; }

        [Column("Destination"), Required]
        public string Destination { get; set; }

        [Column("StartDate"), Required]
        public DateTime StartDate { get; set; }

        [Column("EndDate"), Required]
        public DateTime EndDate { get; set; }

        [Column("Comment")]
        public string Comment { get; set; }
    }
}
