using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TarsOffice.Data;

namespace TarsOffice.Extensions
{
    public static class SiteDbExtensions
    {
        public static async Task<Site> GetPublicSite(this DbSet<Site> siteSet)
        {
            var site = await siteSet
                .Where(site => site.Name == "Public")
                .SingleOrDefaultAsync();
            if (site == null)
            {
                throw new InvalidOperationException("Unable to find ID for Public site");
            }

            return site;
        }

        public static async Task<Site> FindByDomain(this DbSet<Site> siteSet, string domain)
        {
            var site = await siteSet
                .Where(site => site.EmailDomain == domain)
                .SingleOrDefaultAsync();
            
            return site;
        }
    }
}
