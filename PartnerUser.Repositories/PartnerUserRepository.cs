using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartnerUser.Repositories.DbContext;
using PartnerUser.Repositories.Exceptions;

namespace PartnerUser.Repositories
{
    public class PartnerUserRepository : IPartnerUserRepository
    {
        private readonly PartnerUserDbContext _dbContext;

        public PartnerUserRepository(PartnerUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Domain.Model.PartnerUser> GetAll()
        {
            return _dbContext.PartnerUsers.AsNoTracking();
        }

        public async Task<Guid> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId)
        {
            try
            {
                var partnerUser = new Domain.Model.PartnerUser
                {
                    OfxUserGuid = ofxUserGuid,
                    PartnerAppId = partnerAppId,
                    PartnerUserId = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                await _dbContext.AddAsync(partnerUser);
                await _dbContext.SaveChangesAsync();

                return partnerUser.PartnerUserId;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate entry",
                        StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    throw new DuplicateKeyException(
                        $"Cannot insert duplicate record. PartnerUser with OfxUserGuid {ofxUserGuid} and PartnerAppId {partnerAppId} already exists");
                }
                throw;
            }
        }

        public async Task UpdatePartnerUserAsync(Domain.Model.PartnerUser partnerUser)
        {
            partnerUser.UpdatedDate = DateTime.UtcNow;
            _dbContext.PartnerUsers.Update(partnerUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Domain.Model.PartnerUser> GetPartnerUserAsync(Guid partnerUserId)
        {
            return await FindByCondition(pu => pu.PartnerUserId == partnerUserId).FirstOrDefaultAsync();
        }

        private IQueryable<Domain.Model.PartnerUser> FindByCondition(Expression<Func<Domain.Model.PartnerUser, bool>> expression)
        {
            return GetAll().Where(expression);
        }
    }
}
