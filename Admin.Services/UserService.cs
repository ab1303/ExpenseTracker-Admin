using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Repositories;
using Admin.Repositories.Exceptions;
using Admin.Services.interfaces;
using Admin.Services.Results;

namespace Admin.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<HttpServiceResult<Guid>> AddUserAsync(Guid userGuid, string email)
        {
            try
            {
                var userId = await _userRepository.AddUserAsync(userGuid, email);

                return new HttpServiceResult<Guid>
                {
                    Status = ServiceResultStatus.Success,
                    Model = userId
                };

            }
            catch (DuplicateKeyException e)
            {
                return new HttpServiceResult<Guid>
                {
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Status = ServiceResultStatus.Failure,
                    Error = new Error { ErrorCode = ErrorCodes.DuplicateUserInsert, SystemMessage = e.Message }
                };
            }
        }

        public async Task<HttpServiceResult<Admin.Domain.Model.User>> GetUserAsync(Guid partnerUserId)
        {
            var partnerUser = await _userRepository.GetUserAsync(partnerUserId);

            if (partnerUser == null)
            {
                return new HttpServiceResult<Admin.Domain.Model.User>
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Status = ServiceResultStatus.Failure,
                    Error = new Error
                    {
                        ErrorCode = ErrorCodes.UserNotFound,
                        SystemMessage = $"Partner user not found for PartnerUserId {partnerUserId}"
                    }
                };
            }

            return new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = partnerUser
            };
        }

        public async Task<ServiceResult> UpdateUserAsync(Admin.Domain.Model.User user)
        {
            await _userRepository.UpdateUserAsync(user);

            return new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };
        }
    }
}
