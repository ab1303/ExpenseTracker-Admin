using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Admin.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Admin.Repositories.DbContext;

namespace Admin.Repositories
{
    public class PartnerUserRepository : IPartnerUserRepository
    {
        private readonly PartnerUserDbContext _dbContext;

        public PartnerUserRepository(PartnerUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Admin.Domain.Model.PartnerUser> GetAll()
        {
            return _dbContext.PartnerUsers.AsNoTracking();
        }

        public async Task<Guid> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId)
        {
            try
            {
                var partnerUser = new Admin.Domain.Model.PartnerUser
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

        public async Task UpdatePartnerUserAsync(Admin.Domain.Model.PartnerUser partnerUser)
        {
            partnerUser.UpdatedDate = DateTime.UtcNow;
            _dbContext.PartnerUsers.Update(partnerUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Admin.Domain.Model.PartnerUser> GetPartnerUserAsync(Guid partnerUserId)
        {
            return await FindByCondition(pu => pu.PartnerUserId == partnerUserId).FirstOrDefaultAsync();
        }

        private IQueryable<Admin.Domain.Model.PartnerUser> FindByCondition(Expression<Func<Admin.Domain.Model.PartnerUser, bool>> expression)
        {
            return GetAll().Where(expression);
        }
    }
}
