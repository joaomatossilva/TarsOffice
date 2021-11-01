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
        public bool IsAnyMine => TeamBookings.Any(x => x.ItsMe);

        public class TeamMemberBooking
        {
            public Guid Id { get; set; }
            public bool ItsMe { get; set; }
            public User User { get; set; }
            public BookingStatus Status { get; set; }
        }
    }
}
