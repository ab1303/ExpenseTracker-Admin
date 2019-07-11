using System;
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
    public class CreatePartnerUserTests : TestBase
    {
        private readonly TestContext _sut;

        public CreatePartnerUserTests(TestContext sut)
        {
            _sut = sut;
        }

        [Fact(DisplayName = "CreatePartnerUser Should Create PartnerUser With Valid Request")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Valid_Request()
        {
            //Arrange
            var partnerUserRequest = new CreatePartnerUserRequest
            {
                PartnerAppId = Guid.NewGuid(),
                OfxUserGuid = Guid.NewGuid()
            };

            OfxUserGuidPartnerAppIdListForCleanUp.Add(partnerUserRequest.OfxUserGuid, partnerUserRequest.PartnerAppId);

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            var createPartnerUserResponse = await DeserializeHttpResponseMessageContentAsync<CreatePartnerUserResponse>(httpResponseMessage);

            Assert.NotEqual(Guid.Empty, createPartnerUserResponse.PartnerUserId);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Not Create PartnerUser With Null OfxUserGuid")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Null_OfxUserGuid()
        {
            //Arrange
            var partnerUserRequest = new CreatePartnerUserRequest
            {
                PartnerAppId = Guid.NewGuid()
            };

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Not Create PartnerUser With Null PartnerAppId")]
        public async Task CreatePartnerUser_Should_Create_PartnerUser_With_Null_PartnerAppId()
        {
            //Arrange
            var partnerUserRequest = new CreatePartnerUserRequest
            {
                OfxUserGuid = Guid.NewGuid()
            };

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            var httpResponseMessage = await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Not Allow Duplicate OfxUserId PartnerAppId Combination")]
        public async Task CreatePartnerUser_Should_Not_Allow_Duplicate_OfxUserId_PartnerAppId_Combination()
        {
            //Arrange
            var partnerUserRequest = new CreatePartnerUserRequest
            {
                PartnerAppId = Guid.NewGuid(),
                OfxUserGuid = Guid.NewGuid()
            };

            OfxUserGuidPartnerAppIdListForCleanUp.Add(partnerUserRequest.OfxUserGuid, partnerUserRequest.PartnerAppId);

            var partnerUserClient = new PartnerUserClient(_sut.Client);

            //Act
            await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            var httpResponseMessage = await partnerUserClient.CreatePartnerUserAsync(partnerUserRequest);

            //Assert
            Assert.Equal(HttpStatusCode.Conflict, httpResponseMessage.StatusCode);
        }
    }
}
