using System;
using System.Security.Claims;

namespace TarsOffice.Extensions
{
    public static class UserExtensions
    {
        public const string SiteClaimType = "https://meetmy.team/claim_types/site";

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

        public static string GetSite(this ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = claimsPrincipal ?? throw new ArgumentNullException(nameof(claimsPrincipal));
            return claimsPrincipal.FindFirstValue(SiteClaimType);
        }
    }
}
