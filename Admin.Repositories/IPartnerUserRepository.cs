using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Repositories
{
    public interface IPartnerUserRepository
    {

        IQueryable<Admin.Domain.Model.PartnerUser> GetAll();

        Task<Guid> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId);
        Task<Admin.Domain.Model.PartnerUser> GetPartnerUserAsync(Guid partnerUserId);
        Task UpdatePartnerUserAsync(Admin.Domain.Model.PartnerUser partnerUser);
    }
}
