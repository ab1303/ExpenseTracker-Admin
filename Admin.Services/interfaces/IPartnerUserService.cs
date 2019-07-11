using System;
using System.Threading.Tasks;
using Admin.Services.Results;

namespace Admin.Services.interfaces
{
    public interface IPartnerUserService
    {
        Task<HttpServiceResult<Guid>> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId);
        Task<HttpServiceResult<Admin.Domain.Model.PartnerUser>> GetPartnerUserAsync(Guid partnerUserId);
        Task<ServiceResult> UpdatePartnerUserAsync(Admin.Domain.Model.PartnerUser partnerUser);
    }
}