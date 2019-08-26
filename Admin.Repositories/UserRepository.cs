using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Admin.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using Admin.Repositories.DbContext;

namespace Admin.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AdminDbContext _dbContext;

        public UserRepository(AdminDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Admin.Domain.Model.User> GetAll()
        {
            return _dbContext.Users.AsNoTracking();
        }

        public async Task<Guid> AddUserAsync(Guid userGuid, string email)
        {
            try
            {
                var user = new Domain.Model.User
                {
                    UserId = userGuid,
                    Email = email
                };

                await _dbContext.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return user.UserId;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("duplicate entry",
                        StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    throw new DuplicateKeyException(
                        $"Cannot insert duplicate record.");
                }
                throw;
            }
        }

        public async Task UpdateUserAsync(Domain.Model.User user)
        {
            user.UpdatedDate = DateTime.UtcNow;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Admin.Domain.Model.User> GetUserAsync(Guid userId)
        {
            return await FindByCondition(pu => pu.UserId == userId).FirstOrDefaultAsync();
        }

        private IQueryable<Admin.Domain.Model.User> FindByCondition(Expression<Func<Admin.Domain.Model.User, bool>> expression)
        {
            return GetAll().Where(expression);
        }
    }
}
