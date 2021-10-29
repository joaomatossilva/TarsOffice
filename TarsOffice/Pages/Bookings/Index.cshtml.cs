using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;

namespace TarsOffice.Pages.Bookings
{
    public class IndexModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public IndexModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Booking { get;set; }

        public async Task OnGetAsync()
        {
            Booking = await _context.Bookings.ToListAsync();
        }
    }
}
