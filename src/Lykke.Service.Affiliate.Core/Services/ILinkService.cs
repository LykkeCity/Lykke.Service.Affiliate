using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface ILinkService
    {
        Task<LinkResult> CreateNewLink(string clientId, string redirectUrl);
        Task<IEnumerable<LinkResult>> GetLinks(string clientId);
    }

    public class LinkResult
    {
        public string Url { get; set; }
        public string RedirectUrl { get; set; }
    }
}
