using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Repositories;
using Admin.Repositories.Exceptions;
using Admin.Services.interfaces;
using Admin.Services.Results;

namespace Admin.Services
{
    public class PartnerUserService : IPartnerUserService
    {
        private readonly IPartnerUserRepository _partnerUserRepository;

        public PartnerUserService(IPartnerUserRepository partnerUserRepository)
        {
            _partnerUserRepository = partnerUserRepository;
        }

        public async Task<HttpServiceResult<Guid>> AddPartnerUserAsync(Guid ofxUserGuid, Guid partnerAppId)
        {
            try
            {
                var partnerUserId = await _partnerUserRepository.AddPartnerUserAsync(ofxUserGuid, partnerAppId);

                return new HttpServiceResult<Guid>
                {
                    Status = ServiceResultStatus.Success,
                    Model = partnerUserId
                };

            }
            catch (DuplicateKeyException e)
            {
                return new HttpServiceResult<Guid>
                {
                    HttpStatusCode = HttpStatusCode.Conflict,
                    Status = ServiceResultStatus.Failure,
                    Error = new Error { ErrorCode = ErrorCodes.DuplicatePartnerUserInsert, SystemMessage = e.Message }
                };
            }
        }

        public async Task<HttpServiceResult<Admin.Domain.Model.PartnerUser>> GetPartnerUserAsync(Guid partnerUserId)
        {
            var partnerUser = await _partnerUserRepository.GetPartnerUserAsync(partnerUserId);

            if (partnerUser == null)
            {
                return new HttpServiceResult<Admin.Domain.Model.PartnerUser>
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                    Status = ServiceResultStatus.Failure,
                    Error = new Error
                    {
                        ErrorCode = ErrorCodes.PartnerUserNotFound,
                        SystemMessage = $"Partner user not found for PartnerUserId {partnerUserId}"
                    }
                };
            }

            return new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                Status = ServiceResultStatus.Success,
                Model = partnerUser
            };
        }

        public async Task<ServiceResult> UpdatePartnerUserAsync(Admin.Domain.Model.PartnerUser partnerUser)
        {
            await _partnerUserRepository.UpdatePartnerUserAsync(partnerUser);

            return new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };
        }
    }
}
