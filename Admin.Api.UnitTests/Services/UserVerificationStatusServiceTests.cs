using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Admin.Domain.Model;
using Admin.Services;
using Moq;
using Newtonsoft.Json;
using Admin.Infrastructure.HttpClients;
using Serilog;
using Xunit;
using Error = Admin.Services.Results.Error;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Admin.Api.UnitTests.Services
{
    public class UserVerificationStatusServiceTests
    {
        private readonly Mock<IBslApiClient> _bslApiClientMock;
        private readonly UserVerificationStatusService _userVerificationStatusService;

        public UserVerificationStatusServiceTests()
        {
            _bslApiClientMock = new Mock<IBslApiClient>();
            var loggerMock = new Mock<ILogger>();
            _userVerificationStatusService = new UserVerificationStatusService(_bslApiClientMock.Object, loggerMock.Object);
        }

        [Fact(DisplayName = "GetIsUserReadyToDealAsync Should Return Success On Successful Status Retrieval")]
        public void GetIsUserReadyToDealAsync_Should_Return_Success_On_Successful_Status_Retrieval()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            const string token = "ValidToken";

            var userVerificationStatus = new UserVerificationStatus
            {
                SocialSecurityNumberRequired = false,
                VerificationDocumentsSubmitted = true,
                VerificationStatus = "Verified"
            };

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(userVerificationStatus))
            };

            _bslApiClientMock
                .Setup(bslApiClient => bslApiClient.SendAsync(It.IsAny<string>(), HttpMethod.Get, null, token))
                .Returns(Task.FromResult(httpResponseMessage));

            //Act
            var result = _userVerificationStatusService.GetIsUserReadyToDealAsync(ofxUserGuid, token).Result;

            //Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Model);
        }

        [Fact(DisplayName = "GetIsUserReadyToDealAsync Should Return Failure On Http Error")]
        public void GetIsUserReadyToDealAsync_Should_Return_Failure_On_Http_Error()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            const string token = "ValidToken";

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("500"),
                ReasonPhrase = "Internal Server Error"
            };

            _bslApiClientMock
                .Setup(bslApiClient => bslApiClient.SendAsync(It.IsAny<string>(), HttpMethod.Get, null, token))
                .Returns(Task.FromResult(httpResponseMessage));

            //Act
            var result = _userVerificationStatusService.GetIsUserReadyToDealAsync(ofxUserGuid, token).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, result.HttpStatusCode);
            Assert.Equal("500", result.Error.ErrorCode);
            Assert.Equal(httpResponseMessage.ReasonPhrase, result.Error.SystemMessage);
        }

        [Fact(DisplayName = "GetIsUserReadyToDealAsync Should Return Failure On Bsl Error")]
        public void GetIsUserReadyToDealAsync_Should_Return_Failure_On_Bsl_Error()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            const string token = "ValidToken";

            var bslError = new Error {ErrorCode = "US:AllowedCurrencies:0002", SystemMessage = "User does not exist" };

            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(JsonConvert.SerializeObject(bslError))
            };

            _bslApiClientMock
                .Setup(bslApiClient => bslApiClient.SendAsync(It.IsAny<string>(), HttpMethod.Get, null, token))
                .Returns(Task.FromResult(httpResponseMessage));

            //Act
            var result = _userVerificationStatusService.GetIsUserReadyToDealAsync(ofxUserGuid, token).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(HttpStatusCode.Forbidden, result.HttpStatusCode);
            Assert.Equal(bslError.ErrorCode, result.Error.ErrorCode);
            Assert.Equal(bslError.SystemMessage, result.Error.SystemMessage);
        }
    }
}
