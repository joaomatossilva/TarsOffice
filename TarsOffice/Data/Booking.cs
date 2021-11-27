using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TarsOffice.Data
{
    public class Booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public User User { get; set; } 

        [ForeignKey("User")]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public BookingStatus Status { get; set; }

        public Site Site { get; set; }

        [ForeignKey("Site")]
        public Guid SiteId { get; set; }
    }
}
