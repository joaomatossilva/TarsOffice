using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Services.Abstractions;

namespace TarsOffice.Pages.Teams
{
    public class CreateModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public CreateModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Team Team { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var currentSiteId = User.GetSite();
            var newTeam = new Team()
            {
                SiteId = currentSiteId
            };

            if (await TryUpdateModelAsync<Team>(newTeam, "team", s => s.Name))
            {
                newTeam.Members.Add(new TeamMember
                {
                    IsAdmin = true,
                    UserId = User.GetId()
                });
                _context.Teams.Add(newTeam);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Index");
            }

            return Page();
        }
    }
}
