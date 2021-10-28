using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TarsOffice.Extensions
{
    public static class UserExtensions
    {
        public static string GetId(this ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = claimsPrincipal ?? throw new ArgumentNullException(nameof(claimsPrincipal));

            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId is null)
            {
                throw new InvalidOperationException("Invalid Id on principal");
            }

            return userId;
        }
    }
}
