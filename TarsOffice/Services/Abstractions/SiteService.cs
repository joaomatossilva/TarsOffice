using System;
using Microsoft.AspNetCore.Http;
using TarsOffice.Extensions;

namespace TarsOffice.Services.Abstractions
{
    public class SiteService : ISiteService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public SiteService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public Guid GetCurrentSite()
        {
            var siteClaim = httpContextAccessor.HttpContext?.User.GetSite();
            if (Guid.TryParse(siteClaim, out Guid siteId))
            {
                return siteId;
            }

            throw new InvalidOperationException("There is no Site on user claims");
        }
    }
}
