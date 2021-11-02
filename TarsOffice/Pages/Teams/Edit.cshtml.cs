using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;

namespace TarsOffice.Pages.Teams
{
    public class EditModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public EditModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(team => team.Members)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Team == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (!Team.Members.Any(member => member.UserId == userId && member.IsAdmin))
            {
                return Forbid();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            var team = await _context.Teams
                .Include(team => team.Members)
                .FirstOrDefaultAsync(m => m.Id == Team.Id);

            if (team == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (!team.Members.Any(member => member.UserId == userId && member.IsAdmin))
            {
                return Forbid();
            }

            if(await TryUpdateModelAsync(team, "team", t => t.Name))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(Team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToPage("../Index");
            }

            return Page();
        }

        private bool TeamExists(Guid id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}
