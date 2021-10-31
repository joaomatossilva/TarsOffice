using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Viewmodel;

namespace TarsOffice.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public CreateModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(DateTime? date)
        {
            Booking = new NewBooking();
            if(date != null)
            {
                Booking.Date = date.Value;
            }
            else
            {
                Booking.Date = DateTime.Today.AddDays(1);
            }

            return Page();
        }

        [BindProperty]
        public NewBooking Booking { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.GetId();
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            var newBooking = new Booking
            {
                User = user
            };
            if(await TryUpdateModelAsync<Booking>(newBooking, "booking", s => s.Date, s =>s.Status))
            {
                //truncate date
                newBooking.Date = newBooking.Date.Date;
                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Index");
            }

            return Page();
        }
    }
}
