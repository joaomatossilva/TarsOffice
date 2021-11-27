using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TarsOffice.Data
{
    public class Team
    {
        public Team()
        {
            Members = new List<TeamMember>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<TeamMember> Members { get; set; }

        public Site Site { get; set; }

        [ForeignKey("Site")]
        public Guid SiteId { get; set; }
    }
}
