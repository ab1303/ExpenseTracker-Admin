using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Newtonsoft.Json;
using Admin.Api.IntegrationTests.RestClients;
using Admin.Api.IntegrationTests.Setup;
using Xunit;

namespace Admin.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class UpdateUserTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _accessToken;

        public UpdateUserTests(TestContext sut)
        {
            _sut = sut;
            var bslAuthenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _accessToken = bslAuthenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "UpdateUser Should Return NotFound Status Code If User Not Found By PartnerId")]
        public async Task GetUser_Should_Return_NotFound_Status_Code_If_User_Not_Found_By_PartnerId()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUserClient = new UserClient(_sut.Client);

            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "New FirstName"
            };

            //Act
            var httpResponseMessage = await partnerUserClient.UpdateUserAsync(partnerUserId, updateUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "UpdateUser Should Return BadRequest Status Code If Email Not Sent In Request")]
        public async Task UpdateUser_Should_Return_BadRequest_Status_Code_If_BeneficiaryId_Not_Sent_In_Request()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var partnerUserClient = new UserClient(_sut.Client);

            var updateUserRequest = new UpdateUserRequest();

            //Act
            var httpResponseMessage = await partnerUserClient.UpdateUserAsync(partnerUserId, updateUserRequest
                , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "UpdateUser Should Update User Email With Valid Request")]
        public async Task UpdateUser_Should_Update_User_BeneficiaryId_With_Valid_Request()
        {
            //Arrange
            var userRequest = new CreateUserRequest
            {
                Email = "email@test.com",
                UserGuid = Constants.Un10UserGuid
            };
            UserGuidListForCleanUp.Add(userRequest.UserGuid, userRequest.Email);

            var partnerUserClient = new UserClient(_sut.Client);

            var httpResponseMessageCreateUser = await partnerUserClient.CreateUserAsync(userRequest);

            var createUserResponse = await DeserializeHttpResponseMessageContentAsync<CreateUserResponse>
                (httpResponseMessageCreateUser);

            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "TestUser"
            };

            //Act
            var httpResponseMessageUpdateUser = await partnerUserClient.UpdateUserAsync(
                createUserResponse.UserId, updateUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageUpdateUser.StatusCode);

            var httpResponseMessageGetUser = await partnerUserClient
                .GetUserAsync(createUserResponse.UserId, _accessToken);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetUser.StatusCode);
            var userResponse = await DeserializeHttpResponseMessageContentAsync<UserResponse>
                (httpResponseMessageGetUser);

            Assert.Equal(updateUserRequest.FirstName, userResponse.FirstName);
        }

        [Fact(DisplayName = "UpdateUser Should Allow Update User Email To Null")]
        public async Task UpdateUser_Should_Allow_Update_User_BeneficiaryId_To_Null()
        {
            //Arrange
            var userRequest = new CreateUserRequest
            {
                Email = "email@test.com",
                UserGuid = Constants.Un10UserGuid
            };

            UserGuidListForCleanUp.Add(userRequest.UserGuid, userRequest.Email);

            var partnerUserClient = new UserClient(_sut.Client);

            var httpResponseMessageCreateUser = await partnerUserClient.CreateUserAsync(userRequest);

            var createUserResponse = await DeserializeHttpResponseMessageContentAsync<CreateUserResponse>
                (httpResponseMessageCreateUser);

            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            };

            //Act
            await partnerUserClient.UpdateUserAsync(
                createUserResponse.UserId, updateUserRequest);

            var httpResponseMessageUpdateUser = await partnerUserClient.UpdateUserAsync(
                createUserResponse.UserId, new UpdateUserRequest{ FirstName  = null },
                new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Include });

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageUpdateUser.StatusCode);

            var httpResponseMessageGetUser = await partnerUserClient
                .GetUserAsync(createUserResponse.UserId, _accessToken);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetUser.StatusCode);
            var userResponse = await DeserializeHttpResponseMessageContentAsync<UserResponse>
                (httpResponseMessageGetUser);

            Assert.Null(userResponse.FirstName);
        }
    }
}
