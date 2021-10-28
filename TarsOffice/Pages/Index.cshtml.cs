using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Viewmodel;

namespace TarsOffice.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            this.context = context;
            _logger = logger;
        }

        public IList<MyTeams> MyTeams { get; set; }
        public IList<Booking> MyUpcommingBookings { get; set; }

        public async Task OnGet()
        {
            var myId = User.GetId();
            MyTeams = await context.Teams
                .Where(x => x.Members.Any(m => m.User.Id == myId))
                .Select(team => new MyTeams
                {
                    Id = team.Id,
                    Name = team.Name
                })
                .ToListAsync();

            var startDate = DateTime.Today;
            MyUpcommingBookings = await context.Bookings
                .Where(booking => booking.User.Id == myId && booking.Date >= startDate)
                .ToListAsync();
        }
    }
}
