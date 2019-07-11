using System;
using System.Net;
using System.Threading.Tasks;
using PartnerUser.Repositories;
using PartnerUser.Repositories.Exceptions;
using PartnerUser.Services.Interfaces;
using PartnerUser.Services.Results;

namespace PartnerUser.Services
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

        public async Task<HttpServiceResult<Domain.Model.PartnerUser>> GetPartnerUserAsync(Guid partnerUserId)
        {
            var partnerUser = await _partnerUserRepository.GetPartnerUserAsync(partnerUserId);

            if (partnerUser == null)
            {
                return new HttpServiceResult<Domain.Model.PartnerUser>
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

            return new HttpServiceResult<Domain.Model.PartnerUser>
            {
                Status = ServiceResultStatus.Success,
                Model = partnerUser
            };
        }

        public async Task<ServiceResult> UpdatePartnerUserAsync(Domain.Model.PartnerUser partnerUser)
        {
            await _partnerUserRepository.UpdatePartnerUserAsync(partnerUser);

            return new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };
        }
    }
}
