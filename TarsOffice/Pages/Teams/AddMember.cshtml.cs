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
    public class AddMember : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;
        private readonly UserManager<User> userManager;

        public AddMember(TarsOffice.Data.ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(x => x.Members)
                .ThenInclude(y => y.User)
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

        public Team Team { get; set; }

        [BindProperty]
        public NewTeamMember TeamMember { get; set; }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Team = await _context.Teams
                .Include(x => x.Members)
                .ThenInclude(y => y.User)
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

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == TeamMember.Email);
            if(user == null)
            {
                user = new User { UserName = TeamMember.Email, Email = TeamMember.Email, EmailConfirmed = true };
                var result = await userManager.CreateAsync(user);
                if(!result.Succeeded)
                {
                    return Page();
                }
            }

            var newTeamMember = new TeamMember
            {
                User = user,
                IsAdmin = TeamMember.IsAdmin,
                Team = Team
            };

            _context.TeamMembers.Add(newTeamMember);
            await _context.SaveChangesAsync();
            return RedirectToPage("Details", new { Id = id });
        }
    }
}
