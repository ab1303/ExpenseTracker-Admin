using System;
using System.Threading.Tasks;
using Admin.Services.Results;

namespace Admin.Services.interfaces
{
    public interface IAdminService
    {
        Task<HttpServiceResult<Guid>> AddUserAsync(Guid userGuid, string email);
        Task<HttpServiceResult<Domain.Model.User>> GetUserAsync(Guid userId);
        Task<ServiceResult> UpdateUserAsync(Domain.Model.User user);
    }
}