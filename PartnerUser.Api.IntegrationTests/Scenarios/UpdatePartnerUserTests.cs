using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PartnerUser.Api.IntegrationTests.RestClients;
using PartnerUser.Api.IntegrationTests.Setup;
using PartnerUser.Api.Requests;
using PartnerUser.Api.Responses;
using Xunit;

namespace PartnerUser.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class UpdatePartnerUserTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _bslAccessToken;

        public UpdatePartnerUserTests(TestContext sut)
        {
            _sut = sut;
            var bslAuthenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _bslAccessToken = bslAuthenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return NotFound Status Code If PartnerUser Not Found By PartnerId")]
        public async Task GetPartnerUser_Should_Return_NotFound_Status_Code_If_PartnerUser_Not_Found_By_PartnerId()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            var updatePartnerUserRequest = new UpdatePartnerUserRequest
            {
                BeneficiaryId = Guid.NewGuid()
            };

            //Act
            var httpResponseMessage = await partnerUserClient.UpdatePartnerUserAsync(partnerUserId, updatePartnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return BadRequest Status Code If BeneficiaryId Not Sent In Request")]
        public async Task UpdatePartnerUser_Should_Return_BadRequest_Status_Code_If_BeneficiaryId_Not_Sent_In_Request()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            var updatePartnerUserRequest = new UpdatePartnerUserRequest();

            //Act
            var httpResponseMessage = await partnerUserClient.UpdatePartnerUserAsync(partnerUserId, updatePartnerUserRequest
                , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Update PartnerUser BeneficiaryId With Valid Request")]
        public async Task UpdatePartnerUser_Should_Update_PartnerUser_BeneficiaryId_With_Valid_Request()
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

            var updatePartnerUserRequest = new UpdatePartnerUserRequest
            {
                BeneficiaryId = Guid.NewGuid()
            };

            //Act
            var httpResponseMessageUpdatePartnerUser = await partnerUserClient.UpdatePartnerUserAsync(
                createPartnerUserResponse.PartnerUserId, updatePartnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageUpdatePartnerUser.StatusCode);

            var httpResponseMessageGetPartnerUser = await partnerUserClient
                .GetPartnerUserAsync(createPartnerUserResponse.PartnerUserId, _bslAccessToken);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetPartnerUser.StatusCode);
            var partnerUserResponse = await DeserializeHttpResponseMessageContentAsync<PartnerUserResponse>
                (httpResponseMessageGetPartnerUser);

            Assert.Equal(updatePartnerUserRequest.BeneficiaryId, partnerUserResponse.BeneficiaryId);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Allow Update PartnerUser BeneficiaryId To Null")]
        public async Task UpdatePartnerUser_Should_Allow_Update_PartnerUser_BeneficiaryId_To_Null()
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

            var updatePartnerUserRequest = new UpdatePartnerUserRequest
            {
                BeneficiaryId = Guid.NewGuid()
            };

            //Act
            await partnerUserClient.UpdatePartnerUserAsync(
                createPartnerUserResponse.PartnerUserId, updatePartnerUserRequest);

            var httpResponseMessageUpdatePartnerUser = await partnerUserClient.UpdatePartnerUserAsync(
                createPartnerUserResponse.PartnerUserId, new UpdatePartnerUserRequest{ BeneficiaryId  = null },
                new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Include });

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageUpdatePartnerUser.StatusCode);

            var httpResponseMessageGetPartnerUser = await partnerUserClient
                .GetPartnerUserAsync(createPartnerUserResponse.PartnerUserId, _bslAccessToken);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetPartnerUser.StatusCode);
            var partnerUserResponse = await DeserializeHttpResponseMessageContentAsync<PartnerUserResponse>
                (httpResponseMessageGetPartnerUser);

            Assert.Null(partnerUserResponse.BeneficiaryId);
        }
    }
}
