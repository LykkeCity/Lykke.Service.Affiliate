using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.Core.Services
{
    public interface IReferralService
    {
        Task<IEnumerable<IReferral>> GetAllReferrals();
    }
}
