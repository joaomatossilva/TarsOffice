using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Viewmodel;

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
        public IList<TeamDayBookings> TeamBookings { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
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
            if (!Team.Members.Any(member => member.UserId == userId))
            {
                return Forbid();
            }

            var startDate = DateTime.Today;
            var lastDate = startDate.AddDays(7);
            var nextTeamBookings = await _context.Bookings
                .Include(booking => booking.User)
                .Where(booking => booking.Date <= lastDate && booking.Date >= startDate)
                .Join(_context.TeamMembers, b => b.User, t => t.User, (booking, teamMember) => new { booking, teamMember })
                .Where(join => join.teamMember.Team.Id == id)
                .Select(join => join.booking)
                .ToListAsync();

            TeamBookings = new List<TeamDayBookings>();
            for(var date = startDate; date <= lastDate; date = date.AddDays(1))
            {
                var bookings = nextTeamBookings.Where(b => b.Date == date);
                TeamBookings.Add(new TeamDayBookings
                {
                    Date = date,
                    TeamBookings = bookings.Select(booking => new TeamDayBookings.TeamMemberBooking
                    {
                        User = booking.User,
                        Status = booking.Status
                    }).ToList()
                });
            }

            return Page();
        }
    }
}
