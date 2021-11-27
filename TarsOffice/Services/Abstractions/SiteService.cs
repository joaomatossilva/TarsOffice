using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;
using TarsOffice.Extensions;

namespace TarsOffice.Services.Abstractions
{
    public class SiteService : ISiteService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext applicationDbContext;

        private static readonly ConcurrentDictionary<Guid, Site> SiteCache = new ConcurrentDictionary<Guid, Site>();

        public SiteService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext applicationDbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Site> GetCurrentSite()
        {
            var siteId = httpContextAccessor.HttpContext.User.GetSite();
            var site = await GetSite(siteId);
            return site ?? throw new InvalidOperationException("Unable to find current Site");
        }

        public async Task<Site> GetSite(Guid id)
        {
            if(SiteCache.TryGetValue(id, out var site))
            {
                return site;
            }

            site = await applicationDbContext.Sites.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return SiteCache.GetOrAdd(id, site);
        }

    }
}
