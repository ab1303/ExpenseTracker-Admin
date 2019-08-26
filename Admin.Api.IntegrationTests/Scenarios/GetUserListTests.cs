using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Admin.Api.IntegrationTests.RestClients;
using Admin.Api.IntegrationTests.Setup;
using Xunit;

namespace Admin.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class GetUserListTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _accessToken;

        public GetUserListTests(TestContext sut)
        {
            _sut = sut;
            var bslAuthenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _accessToken = bslAuthenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "GetUserList Should Return Empty List If No Users Found")]
        public async Task GetPartnerUserList_Should_Return_Empty_List_If_No_PartnerUsers_Found()
        {
            //Arrange
            var userGuid = Guid.NewGuid();

            var partnerUserClient = new UserClient(_sut.Client);

            //Act
            var getPartnerUserListHttpResponseMessage = await partnerUserClient.GetUserListAsync(userGuid, _accessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getPartnerUserListHttpResponseMessage.StatusCode);

            var partnerUserPaginatedResponse = await DeserializeHttpResponseMessageContentAsync<AdminPaginatedResponse>
                (getPartnerUserListHttpResponseMessage);

            Assert.Empty(partnerUserPaginatedResponse.Users);
        }

        [Fact(DisplayName = "GetUserList Should Return User List By Valid UserGuid and FirstName")]
        public async Task GetPartnerUserList_Should_Return_PartnerUser_List_By_Valid_OfxUserGuid_And_PartnerAppId()
        {
            //Arrange
            var createPartnerUserRequest = new CreateUserRequest
            {
                Email = "email@test.com",
                UserGuid = Constants.Un10UserGuid
            };

            UserGuidListForCleanUp.Add(createPartnerUserRequest.UserGuid, createPartnerUserRequest.Email);

            var partnerUserClient = new UserClient(_sut.Client);

            var createPartnerUserHttpResponseMessage = await partnerUserClient.CreateUserAsync(createPartnerUserRequest);

            //Act
            var getPartnerUserListHttpResponseMessage = await partnerUserClient.GetUserListAsync(createPartnerUserRequest.UserGuid, _accessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, getPartnerUserListHttpResponseMessage.StatusCode);

            var createPartnerUserResponse = await DeserializeHttpResponseMessageContentAsync<CreateUserResponse>
                (createPartnerUserHttpResponseMessage);

            var partnerUserPaginatedResponse = await DeserializeHttpResponseMessageContentAsync<AdminPaginatedResponse>
                (getPartnerUserListHttpResponseMessage);

            var listOfPartnerUsers = partnerUserPaginatedResponse.Users;

            var partnerUserResponses = listOfPartnerUsers as UserResponse[] ?? listOfPartnerUsers.ToArray();
            Assert.NotEmpty(partnerUserResponses);
            Assert.Single(partnerUserResponses);
            Assert.Equal(createPartnerUserResponse.UserId, partnerUserResponses[0].UserId);
            Assert.Equal(createPartnerUserRequest.UserGuid, partnerUserResponses[0].UserId);
        }
    }
}
