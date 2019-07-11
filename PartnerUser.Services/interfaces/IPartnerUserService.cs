using System;
using System.Threading.Tasks;
using PartnerUser.Services.Results;

namespace PartnerUser.Services.Interfaces
{
    public interface IPartnerUserService
    {
        Task<HttpServiceResult<Guid>> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId);
        Task<HttpServiceResult<Domain.Model.PartnerUser>> GetPartnerUserAsync(Guid partnerUserId);
        Task<ServiceResult> UpdatePartnerUserAsync(Domain.Model.PartnerUser partnerUser);
    }
}