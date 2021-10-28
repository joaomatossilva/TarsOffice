using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;

namespace TarsOffice.Pages.Teams
{
    public class DetailsModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public DetailsModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams.FirstOrDefaultAsync(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
