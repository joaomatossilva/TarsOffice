using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarsOffice.Data;

namespace TarsOffice.Viewmodel
{
    public class TeamDayBookings
    {
        public DateTime Date { get; set; }

        public IList<TeamMemberBooking> TeamBookings { get; set; }

        public class TeamMemberBooking
        {
            public IdentityUser User { get; set; }
            public BookingStatus Status { get; set; }
        }
    }
}
