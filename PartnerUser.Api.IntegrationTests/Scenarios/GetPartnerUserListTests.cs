using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PartnerUser.Api.IntegrationTests.RestClients;
using PartnerUser.Api.IntegrationTests.Setup;
using PartnerUser.Api.Requests;
using PartnerUser.Api.Responses;
using Xunit;

namespace PartnerUser.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class GetPartnerUserListTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _bslAccessToken;

        public GetPartnerUserListTests(TestContext sut)
        {
            _sut = sut;
            var bslAuthenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _bslAccessToken = bslAuthenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "GetPartnerUserList Should Return Empty List If No PartnerUsers Found")]
        public async Task GetPartnerUserList_Should_Return_Empty_List_If_No_PartnerUsers_Found()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            var partnerAppId = Guid.NewGuid();

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            var getPartnerUserListHttpResponseMessage = await partnerUserClient.GetPartnerUserListAsync(ofxUserGuid, partnerAppId, _bslAccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getPartnerUserListHttpResponseMessage.StatusCode);

            var partnerUserPaginatedResponse = await DeserializeHttpResponseMessageContentAsync<PartnerUsersPaginatedResponse>
                (getPartnerUserListHttpResponseMessage);

            Assert.Empty(partnerUserPaginatedResponse.Users);
        }

        [Fact(DisplayName = "GetPartnerUserList Should Return PartnerUser List By Valid OfxUserGuid and PartnerAppId")]
        public async Task GetPartnerUserList_Should_Return_PartnerUser_List_By_Valid_OfxUserGuid_And_PartnerAppId()
        {
            //Arrange
            var createPartnerUserRequest = new CreatePartnerUserRequest
            {
                PartnerAppId = Guid.NewGuid(),
                OfxUserGuid = Constants.Un10UserGuid
            };

            OfxUserGuidPartnerAppIdListForCleanUp.Add(createPartnerUserRequest.OfxUserGuid, createPartnerUserRequest.PartnerAppId);

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            var createPartnerUserHttpResponseMessage = await partnerUserClient.CreatePartnerUserAsync(createPartnerUserRequest);

            //Act
            var getPartnerUserListHttpResponseMessage = await partnerUserClient.GetPartnerUserListAsync(
                createPartnerUserRequest.OfxUserGuid, createPartnerUserRequest.PartnerAppId, _bslAccessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getPartnerUserListHttpResponseMessage.StatusCode);

            var createPartnerUserResponse = await DeserializeHttpResponseMessageContentAsync<CreatePartnerUserResponse>
                (createPartnerUserHttpResponseMessage);

            var partnerUserPaginatedResponse = await DeserializeHttpResponseMessageContentAsync<PartnerUsersPaginatedResponse>
                (getPartnerUserListHttpResponseMessage);

            var listOfPartnerUsers = partnerUserPaginatedResponse.Users;

            var partnerUserResponses = listOfPartnerUsers as PartnerUserResponse[] ?? listOfPartnerUsers.ToArray();
            Assert.NotEmpty(partnerUserResponses);
            Assert.Single(partnerUserResponses);
            Assert.Equal(createPartnerUserResponse.PartnerUserId, partnerUserResponses[0].PartnerUserId);
            Assert.Equal(createPartnerUserRequest.OfxUserGuid, partnerUserResponses[0].OfxUserGuid);
            Assert.Equal(createPartnerUserRequest.PartnerAppId, partnerUserResponses[0].PartnerAppId);
        }
    }
}
