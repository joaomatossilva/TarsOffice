using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TarsOffice.Viewmodel
{
    public class NewTeamMember
    {
        [Required]
        public string Email { get; set; }

        public bool IsAdmin { get; set; }
    }
}
