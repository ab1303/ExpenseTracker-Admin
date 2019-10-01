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
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserService _adminService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _adminService = new UserService(_userRepositoryMock.Object);
        }

        [Fact(DisplayName = "AddUserAsync Should Return Success On Add")]
        public void AddUserAsync_Should_Return_Success_On_Add()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var email = "email@test.com";
            var newUserGuid = Guid.NewGuid();
            _userRepositoryMock
                .Setup(userRepo => userRepo.AddUserAsync(userGuid, email))
                .Returns(Task.FromResult(newUserGuid));

            //Act
            var result = _adminService.AddUserAsync(userGuid, email).Result;

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newUserGuid, result.Model);
        }

        [Fact(DisplayName = "AddUserAsync Should Return Failure On DuplicateKeyException")]
        public void AddUserAsync_Should_Return_Failure_On_DuplicateKeyException()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var email = "email@test.com";
            const string errorMessage = "cannot insert duplicate key";
            _userRepositoryMock
                .Setup(userRepo => userRepo.AddUserAsync(userGuid, email))
                .Throws(new DuplicateKeyException(errorMessage));

            //Act
            var result = _adminService.AddUserAsync(userGuid, email).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCodes.DuplicateUserInsert, result.Error.ErrorCode);
            Assert.Equal(HttpStatusCode.Conflict, result.HttpStatusCode);
        }

        [Fact(DisplayName = "GetUserAsync Should Return Failure If User Not Found By UserId")]
        public void GetUserAsync_Should_Return_Failure_If_User_Not_Found_By_UserId()
        {
            //Arrange
            var userGuid = Guid.NewGuid();

            //Act
            var result = _adminService.GetUserAsync(userGuid).Result;

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ErrorCodes.UserNotFound, result.Error.ErrorCode);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
        }

        [Fact(DisplayName = "GetUserAsync Should Return Success If User Found By UserId")]
        public void GetUserAsync_Should_Return_Success_If_User_Found_By_UserId()
        {
            //Arrange
            var userGuid = Guid.NewGuid();

            var user = new Domain.Model.User
            {
                UserId = userGuid
            };

            _userRepositoryMock
                .Setup(userRepo => userRepo.GetUserAsync(userGuid))
                .Returns(Task.FromResult(user));

            //Act
            var result = _adminService.GetUserAsync(userGuid).Result;

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(user.UserId, result.Model.UserId);
            
        }

        [Fact(DisplayName = "UpdateUserAsync Should Return Success If User Updated Successful")]
        public async void UpdateUserAsync_Should_Return_Success_If_User_Updated_Successful()
        {
            //Arrange
            var userGuid = Guid.NewGuid();

            var userToSave = new Admin.Domain.Model.User
            {
                UserId = userGuid,
            };

            //Act
            await _adminService.UpdateUserAsync(userToSave);

            //Assert
            _userRepositoryMock
                .Verify(userRepo => userRepo.UpdateUserAsync(It.Is<Admin.Domain.Model.User>(
                    user => user.UserId == userToSave.UserId 
                    && user.Email == userToSave.Email)), Times.Once);
        }
    }
}
