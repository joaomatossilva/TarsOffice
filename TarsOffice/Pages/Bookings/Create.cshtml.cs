using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;
using TarsOffice.Services.Abstractions;
using TarsOffice.Viewmodel;

namespace TarsOffice.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
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
                Booking.Date = DateTime.Today.AddWorkingDays(1);
            }

            return Page();
        }

        [BindProperty]
        public NewBooking Booking { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.GetId();
            var currentSiteId = User.GetSite();
            var user = await _context.Users.FirstAsync(x => x.Id == userId);
            var newBooking = new Booking
            {
                User = user,
                SiteId = currentSiteId
            };

            if(await TryUpdateModelAsync(newBooking, "booking", s => s.Date, s =>s.Status))
            {
                //truncate date
                newBooking.Date = newBooking.Date.Date;
                if (newBooking.Date.IsWorkingDay())
                {
                    _context.Bookings.Add(newBooking);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("../Index");
                }
            }

            return Page();
        }
    }
}
