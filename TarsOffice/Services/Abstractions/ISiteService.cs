using System;
using System.Threading.Tasks;
using TarsOffice.Data;

namespace TarsOffice.Services.Abstractions
{
    public interface ISiteService
    {
        Task<Site> GetCurrentSite();
    }
}
