using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PartnerUser.Repositories
{
    public interface IPartnerUserRepository
    {

        IQueryable<Domain.Model.PartnerUser> GetAll();

        Task<Guid> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId);
        Task<Domain.Model.PartnerUser> GetPartnerUserAsync(Guid partnerUserId);
        Task UpdatePartnerUserAsync(Domain.Model.PartnerUser partnerUser);
    }
}
