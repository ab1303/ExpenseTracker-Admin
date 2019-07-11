using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Admin.Api.IntegrationTests;
using Admin.Api.IntegrationTests.RestClients;
using Admin.Api.IntegrationTests.Scenarios;
using Admin.Api.IntegrationTests.Setup;
using Xunit;

namespace Admin.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class GetPartnerUserTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _bslAccessToken;

        public GetPartnerUserTests(TestContext sut)
        {
            _sut = sut;
            var bslAuthenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _bslAccessToken = bslAuthenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "GetPartnerUser Should Return NotFound Status Code If PartnerUser Not Found By PartnerId")]
        public async Task GetPartnerUser_Should_Return_NotFound_Status_Code_If_PartnerUser_Not_Found_By_PartnerId()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.GetPartnerUserAsync(partnerUserId, _bslAccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "GetPartnerUser Should Return PartnerUser With Valid Request")]
        public async Task GetPartnerUser_Should_Return_PartnerUser_With_Valid_Request()
        {
            //Arrange
            var partnerUserRequest = new CreatePartnerUserRequest
            {
                PartnerAppId = Guid.NewGuid(),
                OfxUserGuid = Constants.Un10UserGuid
            };

            OfxUserGuidPartnerAppIdListForCleanUp.Add(partnerUserRequest.OfxUserGuid, partnerUserRequest.PartnerAppId);

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            var httpResponseMessageCreatePartnerUser = await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            var createPartnerUserResponse = await DeserializeHttpResponseMessageContentAsync<CreatePartnerUserResponse>
                (httpResponseMessageCreatePartnerUser);

            //Act
            var httpResponseMessageGetPartnerUser = await partnerUserClient.GetPartnerUserAsync(createPartnerUserResponse.PartnerUserId, _bslAccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetPartnerUser.StatusCode);
            var partnerUserResponse = await DeserializeHttpResponseMessageContentAsync<PartnerUserResponse>
                (httpResponseMessageGetPartnerUser);

            Assert.Equal(createPartnerUserResponse.PartnerUserId, partnerUserResponse.PartnerUserId);
            Assert.Equal(partnerUserRequest.OfxUserGuid, partnerUserResponse.OfxUserGuid);
            Assert.Equal(partnerUserRequest.PartnerAppId, partnerUserResponse.PartnerAppId);
        }
    }
}
