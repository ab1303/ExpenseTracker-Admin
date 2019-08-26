using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Admin.Api.IntegrationTests.RestClients;
using Admin.Api.IntegrationTests.Scenarios;
using Admin.Api.IntegrationTests.Setup;
using Xunit;

namespace Admin.Api.IntegrationTests.Scenarios
{
    [Collection("api")]
    public class CreateUserTests : TestBase
    {
        private readonly TestContext _sut;

        public CreateUserTests(TestContext sut)
        {
            _sut = sut;
        }

        [Fact(DisplayName = "CreatePartnerUser Should Create User With Valid Request")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Valid_Request()
        {
            //Arrange
            var partnerUserRequest = new CreateUserRequest
            {
                Email = "email@test.com",
                UserGuid = Guid.NewGuid()
            };

            UserGuidListForCleanUp.Add(partnerUserRequest.UserGuid, partnerUserRequest.Email);

            var partnerUserClient = new UserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreateUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            var createPartnerUserResponse = await DeserializeHttpResponseMessageContentAsync<CreateUserResponse>(httpResponseMessage);

            Assert.NotEqual(Guid.Empty, createPartnerUserResponse.UserId);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Not Create User With Null UserGuid")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Null_OfxUserGuid()
        {
            //Arrange
            var partnerUserRequest = new CreateUserRequest
            {
                Email = "email@test.com",
            };

            var partnerUserClient = new UserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreateUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Not Create User With Null FirstName")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Null_PartnerAppId()
        {
            //Arrange
            var partnerUserRequest = new CreateUserRequest
            {
                UserGuid = Guid.NewGuid()
            };

            var partnerUserClient = new UserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreateUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }
    }
}
