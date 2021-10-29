using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TarsOffice.Data;
using TarsOffice.Extensions;

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
            Booking = new Booking();
            if(date != null)
            {
                Booking.Date = date.Value;
            }

            return Page();
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState["Booking.User"].Errors.Clear();
            ModelState["Booking.User"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newBooking = new Booking
            {
                UserId = User.GetId()
            };
            if(await TryUpdateModelAsync<Booking>(newBooking, "booking", s => s.Date, s =>s.Status))
            {
                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();
                return RedirectToPage("../Index");
            }

            return Page();
        }
    }
}
