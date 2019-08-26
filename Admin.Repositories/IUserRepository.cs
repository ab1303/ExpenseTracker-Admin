using System;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Repositories
{
    public interface IUserRepository
    {
        IQueryable<Domain.Model.User> GetAll();
        Task<Guid> AddUserAsync(Guid userGuid, string email);
        Task<Domain.Model.User> GetUserAsync(Guid userId);
        Task UpdateUserAsync(Domain.Model.User user);
    }
}
