using System;
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
    public class GetUserTests : TestBase
    {
        private readonly TestContext _sut;
        private readonly string _accessToken;

        public GetUserTests(TestContext sut)
        {
            _sut = sut;
            var authenticationClient = new AuthServerClient(_sut.AuthServerClient);
            _accessToken = authenticationClient
                .GetApigeeClientCredentialsToken(Constants.Un10UserGuid).Result;
        }

        [Fact(DisplayName = "GetUser Should Return NotFound Status Code If User Not Found By PartnerId")]
        public async Task GetUser_Should_Return_NotFound_Status_Code_If_User_Not_Found_By_PartnerId()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var userClient = new UserClient(_sut.Client);

            //Act
            var httpResponseMessage = await userClient.GetUserAsync(userId, _accessToken);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "GetUser Should Return User With Valid Request")]
        public async Task GetUser_Should_Return_User_With_Valid_Request()
        {
            //Arrange
            var userRequest = new CreateUserRequest
            {
                Email = "email@test.com",
                UserGuid = Constants.Un10UserGuid
            };

            UserGuidListForCleanUp.Add(userRequest.UserGuid, userRequest.Email);

            var userClient = new UserClient(_sut.Client);

            var httpResponseMessageCreateUser = await userClient.CreateUserAsync(userRequest);

            var createUserResponse = await DeserializeHttpResponseMessageContentAsync<CreateUserResponse>
                (httpResponseMessageCreateUser);

            //Act
            var httpResponseMessageGetUser = await userClient.GetUserAsync(createUserResponse.UserId, _accessToken);

            //Assert
            Assert.Equal(HttpStatusCode.OK, httpResponseMessageGetUser.StatusCode);
            var userResponse = await DeserializeHttpResponseMessageContentAsync<UserResponse>
                (httpResponseMessageGetUser);

            Assert.Equal(createUserResponse.UserId, userResponse.UserId);
            Assert.Equal(userRequest.UserGuid, userResponse.UserId);
            Assert.Equal(userRequest.Email, userResponse.FirstName);
        }
    }
}
