using AutoMapper;
using PartnerUser.Api.Responses;

namespace PartnerUser.Api.AutoMapper
{
    public class PartnerUserProfile : Profile
    {
        public PartnerUserProfile()
        {
            CreateMap<Domain.Model.PartnerUser, PartnerUserResponse>();
        }
    }
}
