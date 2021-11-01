using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Viewmodel;

namespace TarsOffice.Pages.Teams
{
    public class RemoveMember : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public RemoveMember(TarsOffice.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(Guid teamId, Guid id)
        {
            if (teamId == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(x => x.Members)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(m => m.Id == teamId);

            if (Team == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (!Team.Members.Any(member => member.UserId == userId && member.IsAdmin))
            {
                return Forbid();
            }

            TeamMember = Team.Members.FirstOrDefault(member => member.Id == id);
            if (TeamMember == null)
            {
                return NotFound();
            }

            return Page();
        }

        public Team Team { get; set; }

        [BindProperty]
        public TeamMember TeamMember { get; set; }

        public async Task<IActionResult> OnPostAsync(Guid teamId, Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(x => x.Members)
                .ThenInclude(y => y.User)
                .FirstOrDefaultAsync(m => m.Id == teamId);

            if (Team == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (!Team.Members.Any(member => member.UserId == userId && member.IsAdmin))
            {
                return Forbid();
            }

            TeamMember = Team.Members.FirstOrDefault(member => member.Id == id);
            if (TeamMember == null)
            {
                return NotFound();
            }

            _context.TeamMembers.Remove(TeamMember);
            await _context.SaveChangesAsync();
            
            return RedirectToPage("Details", new { Id = teamId });
        }
    }
}
