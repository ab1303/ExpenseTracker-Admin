using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PartnerUser.Common;
using PartnerUser.Domain.Model;
using PartnerUser.Infrastructure.HttpClients;
using PartnerUser.Services.Interfaces;
using PartnerUser.Services.Results;
using Serilog;

namespace PartnerUser.Services
{
    public class UserVerificationStatusService : IUserVerificationStatusService
    {
        private readonly IBslApiClient _bslApiClient;
        private readonly ILogger _logger;

        public UserVerificationStatusService(IBslApiClient bslApiClient, ILogger logger)
        {
            _bslApiClient = bslApiClient;
            _logger = logger;
        }

        public async Task<HttpServiceResult<bool>> GetIsUserReadyToDealAsync(Guid ofxUserGuid, string token)
        {
            var response = await _bslApiClient.SendAsync($"{Constants.BslUserApiBaseUrl}/User/{ofxUserGuid}/VerificationStatus",
                HttpMethod.Get, token: token);

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var userVerificationStatus = JsonConvert.DeserializeObject<UserVerificationStatus>(content);
                return new HttpServiceResult<bool>
                {
                    Status = ServiceResultStatus.Success,
                    Model = userVerificationStatus.VerificationStatus == Constants.VerifiedVerificationStatus
                };
            }

            if (!content.Contains("errorcode", StringComparison.InvariantCultureIgnoreCase) ||
                !content.Contains("systemmessage", StringComparison.InvariantCultureIgnoreCase))
            {
                _logger.Error($"GetUserVerificationStatusAsync({ofxUserGuid}) Error({content}, {response.ReasonPhrase})");
                return new HttpServiceResult<bool>
                {
                    Status = ServiceResultStatus.Failure,
                    HttpStatusCode = response.StatusCode,
                    Error = new Error { ErrorCode = content, SystemMessage = response.ReasonPhrase }
                };
            }

            var error = JsonConvert.DeserializeObject<Error>(content);
            _logger.Error($"GetUserVerificationStatusAsync({ofxUserGuid}) Error({error.ErrorCode}, {error.SystemMessage})");
            return new HttpServiceResult<bool>
            {
                Status = ServiceResultStatus.Failure,
                HttpStatusCode = response.StatusCode,
                Error = error
            };
        }
    }
}
