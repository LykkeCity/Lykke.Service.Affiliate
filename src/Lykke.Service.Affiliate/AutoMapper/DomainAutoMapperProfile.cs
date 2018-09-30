using AutoMapper;
using Lykke.Service.Affiliate.Contracts;
using Lykke.Service.Affiliate.Core.Domain.Repositories.Mongo;

namespace Lykke.Service.Affiliate.AutoMapper
{
    internal sealed class DomainAutoMapperProfile : Profile
    {
        public DomainAutoMapperProfile()
        {
            CreateMap<IReferral, ReferralModel>()
                .ForMember(x => x.Id, s => s.MapFrom(o => o.ReferralId));
        }
    }
}
