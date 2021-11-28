using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarsOffice.Data;
using TarsOffice.DayFeatures.Abstractions;
using TarsOffice.Extensions;
using TarsOffice.Services.Abstractions;
using TarsOffice.Viewmodel;

namespace TarsOffice.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly IEnumerable<IDayFeature> dayFeatures;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, IEnumerable<IDayFeature> dayFeatures, ILogger<IndexModel> logger)
        {
            this.context = context;
            this.dayFeatures = dayFeatures;
            _logger = logger;
        }

        public IList<MyTeams> MyTeams { get; set; }
        public IList<TeamDayBookings> TeamBookings { get; set; }

        public async Task OnGet(Guid? teamId)
        {
            var myId = User.GetId();
            var currentSiteId = User.GetSite();
            MyTeams = await context.Teams
                .Where(x => x.Members.Any(m => m.User.Id == myId) && x.SiteId == currentSiteId)
                .Select(team => new MyTeams
                {
                    Id = team.Id,
                    Name = team.Name
                })
                .ToListAsync();

            var myTeamsQuery = context.Teams.Where(team => team.Members.Any(tm => tm.UserId == myId));
            var allTeamMatesQuery = context.TeamMembers
                .Join(myTeamsQuery, x => x.Team, y => y, (member, team) => member)
                .GroupBy(x => x.UserId)
                .Select(x => x.Key);

            var startDate = DateTime.Today;
            if (!startDate.IsWorkingDay())
            {
                startDate = startDate.AddWorkingDays(1);
            }

            var lastDate = startDate.AddDays(13);
            var nextTeamBookings = await context.Bookings
                .Include(booking => booking.User)
                .Where(booking => booking.Date <= lastDate && booking.Date >= startDate && booking.SiteId == currentSiteId)
                .Join(allTeamMatesQuery, b => b.UserId, k => k, (booking, teamMember) => new { booking, teamMember })
                .Select(join => join.booking)
                .ToListAsync();

            TeamBookings = new List<TeamDayBookings>();
            for (var date = startDate; date <= lastDate; date = date.AddWorkingDays(1))
            {
                var bookings = nextTeamBookings.Where(b => b.Date == date);
                TeamBookings.Add(new TeamDayBookings
                {
                    Date = date,
                    DayFeatureTags = dayFeatures.Where(x => x.IsSatisfiedBy(date)).SelectMany(x => x.Render(date)).ToList(),
                    TeamBookings = bookings.Select(booking => new TeamDayBookings.TeamMemberBooking
                    {
                        Id = booking.Id,
                        User = booking.User,
                        ItsMe = booking.User.Id == myId,
                        Status = booking.Status
                    }).ToList()
                });
            }
        }
    }
}
