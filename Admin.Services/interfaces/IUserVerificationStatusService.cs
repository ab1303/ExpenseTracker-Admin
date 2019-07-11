using System;
using System.Threading.Tasks;
using Admin.Services.Results;

namespace Admin.Services.interfaces
{
    public interface IUserVerificationStatusService
    {
        Task<HttpServiceResult<bool>> GetIsUserReadyToDealAsync(Guid ofxUserGuid, string token);
    }
}