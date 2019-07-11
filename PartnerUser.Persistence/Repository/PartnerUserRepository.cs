using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PartnerUser.Persistence.DbContext;

namespace PartnerUser.Persistence.Repository
{
    public class PartnerUserRepository
    {
        private readonly PartnerUserDbContext _dbContext;

        public PartnerUserRepository(PartnerUserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId)
        {
            //try
            //{
                var partnerUser = new Entity.PartnerUser
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
            //}
            //catch (ConditionalCheckFailedException)
            //{
            //    throw new DuplicateKeyException($"Cannot insert duplicate record. PartnerUser with OfxUserGuid {ofxUserGuid} and PartnerAppId {partnerAppId} already exists");
            //}
        }

        public async Task UpdatePartnerUserAsync(Entity.PartnerUser partnerUser)
        {
            partnerUser.UpdatedDate = DateTime.UtcNow;
            _dbContext.PartnerUsers.Update(partnerUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Entity.PartnerUser> GetPartnerUserAsync(Guid partnerUserId)
        {
            var partnerUsers = _dbContext.Set<Entity.PartnerUser>().Where(x => x.PartnerUserId == partnerUserId).AsNoTracking();

            return partnerUsers?.FirstOrDefault();
        }
    }
}
