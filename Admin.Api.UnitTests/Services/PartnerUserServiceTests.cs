using System;
using System.Net;
using System.Threading.Tasks;
using Admin.Repositories;
using Admin.Repositories.Exceptions;
using Admin.Services;
using Admin.Services.Results;
using Moq;
using Xunit;

namespace Admin.Api.UnitTests.Services
{
    public class PartnerUserServiceTests
    {
        private readonly Mock<IPartnerUserRepository> _partnerUserRepositoryMock;
        private readonly PartnerUserService _partnerUserService;

        public PartnerUserServiceTests()
        {
            _partnerUserRepositoryMock = new Mock<IPartnerUserRepository>();
            _partnerUserService = new PartnerUserService(_partnerUserRepositoryMock.Object);
        }

        [Fact(DisplayName = "AddPartnerUserAsync Should Return Success On Add")]
        public void AddPartnerUserAsync_Should_Return_Success_On_Add()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            var partnerAppId = Guid.NewGuid();
            var newPartnerUserGuid = Guid.NewGuid();
            _partnerUserRepositoryMock
                .Setup(partnerUserRepo => partnerUserRepo.AddPartnerUserAsync(ofxUserGuid, partnerAppId))
                .Returns(Task.FromResult(newPartnerUserGuid));

            //Act
            var result = _partnerUserService.AddPartnerUserAsync(ofxUserGuid, partnerAppId).Result;

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newPartnerUserGuid, result.Model);
        }

        [Fact(DisplayName = "AddPartnerUserAsync Should Return Failure On DuplicateKeyException")]
        public void AddPartnerUserAsync_Should_Return_Failure_On_DuplicateKeyException()
        {
            //Arrange
            var ofxUserGuid = Guid.NewGuid();
            var partnerAppId = Guid.NewGuid();
            const string errorMessage = "cannot insert duplicate key";
            _partnerUserRepositoryMock
                .Setup(partnerUserRepo => partnerUserRepo.AddPartnerUserAsync(ofxUserGuid, partnerAppId))
                .Throws(new DuplicateKeyException(errorMessage));

            //Act
            var result = _partnerUserService.AddPartnerUserAsync(ofxUserGuid, partnerAppId).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCodes.DuplicatePartnerUserInsert, result.Error.ErrorCode);
            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
        }

        [Fact(DisplayName = "GetPartnerUserAsync Should Return Failure If PartnerUser Not Found By PartnerUserId")]
        public void GetPartnerUserAsync_Should_Return_Failure_If_PartnerUser_Not_Found_By_PartnerUserId()
        {
            //Arrange
            var partnerUserGuid = Guid.NewGuid();

            //Act
            var result = _partnerUserService.GetPartnerUserAsync(partnerUserGuid).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCodes.PartnerUserNotFound, result.Error.ErrorCode);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact(DisplayName = "GetPartnerUserAsync Should Return Success If PartnerUser Found By PartnerUserId")]
        public void GetPartnerUserAsync_Should_Return_Success_If_PartnerUser_Found_By_PartnerUserId()
        {
            //Arrange
            var partnerUserGuid = Guid.NewGuid();

            var partnerUser = new Admin.Domain.Model.PartnerUser
            {
                PartnerUserId = partnerUserGuid, PartnerAppId = Guid.NewGuid(), OfxUserGuid = Guid.NewGuid()
            };

            _partnerUserRepositoryMock
                .Setup(partnerUserRepo => partnerUserRepo.GetPartnerUserAsync(partnerUserGuid))
                .Returns(Task.FromResult(partnerUser));

            //Act
            var result = _partnerUserService.GetPartnerUserAsync(partnerUserGuid).Result;

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(partnerUser.PartnerUserId, result.Model.PartnerUserId);
            Assert.Equal(partnerUser.OfxUserGuid, result.Model.OfxUserGuid);
            Assert.Equal(partnerUser.PartnerAppId, result.Model.PartnerAppId);
        }

        [Fact(DisplayName = "UpdatePartnerUserAsync Should Return Success If PartnerUser Updated Successful")]
        public async void UpdatePartnerUserAsync_Should_Return_Success_If_PartnerUser_Updated_Successful()
        {
            //Arrange
            var partnerUserGuid = Guid.NewGuid();

            var partnerUserToSave = new Admin.Domain.Model.PartnerUser
            {
                PartnerUserId = partnerUserGuid,
                PartnerAppId = Guid.NewGuid(),
                OfxUserGuid = Guid.NewGuid()
            };

            //Act
            await _partnerUserService.UpdatePartnerUserAsync(partnerUserToSave);

            //Assert
            _partnerUserRepositoryMock
                .Verify(partnerUserRepo => partnerUserRepo.UpdatePartnerUserAsync(It.Is<Admin.Domain.Model.PartnerUser>(
                    partnerUser => partnerUser.PartnerUserId == partnerUserToSave.PartnerUserId 
                    && partnerUser.OfxUserGuid == partnerUserToSave.OfxUserGuid
                    && partnerUser.PartnerAppId == partnerUserToSave.PartnerAppId)), Times.Once);
        }
    }
}
