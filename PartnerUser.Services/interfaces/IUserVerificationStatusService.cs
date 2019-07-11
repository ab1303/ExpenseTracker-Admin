using System;
using System.Threading.Tasks;
using PartnerUser.Services.Results;

namespace PartnerUser.Services.Interfaces
{
    public interface IUserVerificationStatusService
    {
        Task<HttpServiceResult<bool>> GetIsUserReadyToDealAsync(Guid ofxUserGuid, string token);
    }
}