using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;

namespace TarsOffice.Pages.Bookings
{
    public class DeleteModel : PageModel
    {
        private readonly TarsOffice.Data.ApplicationDbContext _context;

        public DeleteModel(TarsOffice.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Booking Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Bookings.FirstOrDefaultAsync(m => m.Id == id);

            if (Booking == null)
            {
                return NotFound();
            }

            var userId = User.GetId();
            if (Booking.UserId != userId)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking = await _context.Bookings.FindAsync(id);

            if (Booking != null)
            {
                var userId = User.GetId();
                if (Booking.UserId != userId)
                {
                    return Forbid();
                }

                _context.Bookings.Remove(Booking);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
