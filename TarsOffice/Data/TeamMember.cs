using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TarsOffice.Data
{
    public class TeamMember
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public IdentityUser User { get; set; }

        [Required]
        public Team Team { get; set; }

        public bool IsAdmin { get; set; }
    }
}
