using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TarsOffice.Data;

namespace TarsOffice.Viewmodel
{
    public class NewBooking
    {
        public DateTime Date { get; set; }

        public BookingStatus Status { get; set; }
    }
}
