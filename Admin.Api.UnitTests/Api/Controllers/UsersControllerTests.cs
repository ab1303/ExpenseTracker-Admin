using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Admin.Api.AutoMapper;
using Admin.Api.Controllers;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Admin.Common.Models;
using Admin.Services.interfaces;
using Admin.Services.Queries;
using Admin.Services.Results;
using Xunit;

namespace Admin.Api.UnitTests.Api.Controllers
{
    public class UsersControllerTests
    {

        private readonly IEnumerable<Admin.Domain.Model.User> _usersData;
        private readonly IEnumerable<(Guid, bool)> _usersVerificationStatus;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IUserVerificationStatusService> _userVerificationStatusMock;
        private readonly Mock<IQueryService> _queryServiceMock;
        private readonly UserController _userController;

        public UsersControllerTests()
        {

            var defaultHttpContext = new DefaultHttpContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultHttpContext);
            var mockedStore = new MockedDataStore();
            _usersData = mockedStore.UsersData();
            _usersVerificationStatus = mockedStore.UsersBslStatusData();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _userServiceMock = new Mock<IUserService>();
            _userVerificationStatusMock = new Mock<IUserVerificationStatusService>();
            _queryServiceMock = new Mock<IQueryService>();

            _userController = new UserController(mapper,
                _userServiceMock.Object, _userVerificationStatusMock.Object, _queryServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = defaultHttpContext }
            };
        }

        #region CreateUser

        [Fact(DisplayName = "CreateUser Should Return UnProcessableEntity StatusCode On AdminService Failure")]
        public void CreateUser_Should_Return_UnProcessableEntity_StatusCode_On_UserService_Failure()
        {
            //Arrange
            var request = new CreateUserRequest
            {
                UserGuid = Guid.NewGuid(),
                Email = "email@test.com"
            };

            var failResult = new HttpServiceResult<Guid>
            {
                HttpStatusCode = HttpStatusCode.UnprocessableEntity,
                Error = new Error { ErrorCode = ErrorCodes.DuplicateUserInsert, SystemMessage = "Failed!" },
                Status = ServiceResultStatus.Failure
            };

            _userServiceMock.Setup(svc => svc.AddUserAsync(request.UserGuid, request.Email))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _userController.CreateUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "CreateUser Should Return Created StatusCode And UserId On AdminService Success")]
        public void CreateUser_Should_Return_Created_StatusCode_And_UserId_On_UserService_Success()
        {
            //Arrange
            var request = new CreateUserRequest
            {
                UserGuid = Guid.NewGuid(),
                Email = "email@test.com"
            };

            var newUserId = Guid.NewGuid();
            var successResult = new HttpServiceResult<Guid>
            {
                Model = newUserId,
                Status = ServiceResultStatus.Success
            };

            _userServiceMock.Setup(svc => svc.AddUserAsync(request.UserGuid, request.Email))
                .Returns(Task.FromResult(successResult));

            //Act
            var response = _userController.CreateUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(newUserId, ((CreateUserResponse)response.Value).UserId);
        }

        #endregion

        #region GetUser

        [Fact(DisplayName = "GetUser Should Return Not Found StatusCode On User Not Found")]
        public void GetUser_Should_Return_Not_Found_StatusCode_On_User_Not_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Domain.Model.User>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.UserNotFound, SystemMessage = "Not found!" }
            };

            _userServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _userController.GetUser(userId).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "GetUser Should Return User If Found")]
        public void GetUser_Should_Return_User_If_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var getUserSuccessResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.User { UserId = userId }
            };

            _userServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(getUserSuccessResult));

            var getIsUserReadyToDealSuccessResult = new HttpServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Model = true
            };

            _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(getIsUserReadyToDealSuccessResult));

            //Act
            var response = _userController.GetUser(userId).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var getUserResponse = response.Value as UserResponse;

            Assert.NotNull(getUserResponse);
            Assert.Equal(getUserSuccessResult.Model.UserId, getUserResponse.UserId);
            Assert.Equal(getUserSuccessResult.Model.Email, getUserResponse.Email);
            Assert.Equal(getUserSuccessResult.Model.FirstName, getUserResponse.FirstName);
        }

        #endregion

        #region UpdateUser

        [Fact(DisplayName = "UpdateUser Should Return Not Found StatusCode On User Not Found")]
        public void UpdateUser_Should_Return_Not_Found_StatusCode_On_User_Not_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.UserNotFound, SystemMessage = "Not found!" }
            };

            _userServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _userController.UpdateUser(userId, new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            }).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "UpdateUser Should Return Bad Request StatusCode On Beneficiary Id Property Not Set")]
        public void UpdateUser_Should_Return_Bad_Request_StatusCode_On_BeneficiaryId_Property_Not_Set()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Status = ServiceResultStatus.Failure,
                Error = new Error
                {
                    ErrorCode = ErrorCodes.RequestPropertyNotSet,
                    SystemMessage = "The Email field must be set to null or a Guid."
                }
            };

            //Act
            var response = _userController.UpdateUser(userId, new UpdateUserRequest()).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(failResult.Error.ErrorCode, ((Error)response.Value).ErrorCode);
            Assert.Equal(failResult.Error.SystemMessage, ((Error)response.Value).SystemMessage);
        }

        [Fact(DisplayName = "UpdateUser Should Return Ok Status Code On Success Update with Email Guid")]
        public void UpdateUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_Guid()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var successGetUserResult = new HttpServiceResult<Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Domain.Model.User
                {
                    UserId = userId,
                }
            };

            _userServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(successGetUserResult));

            var successUpdateUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _userServiceMock.Setup(svc => svc.UpdateUserAsync(successGetUserResult.Model))
                .Returns(Task.FromResult(successUpdateUserResult));

            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            };

            //Act
            var response = _userController.UpdateUser(userId, updateUserRequest).Result as OkResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "UpdateUser Should Return Ok Status Code On Success Update with Email To Null")]
        public void UpdateUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_To_Null()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var successGetUserResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.User
                {
                    UserId = userId,
                }
            };

            _userServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(successGetUserResult));

            var successUpdateUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _userServiceMock.Setup(svc => svc.UpdateUserAsync(successGetUserResult.Model))
                .Returns(Task.FromResult(successUpdateUserResult));

            var updateUserRequest = new UpdateUserRequest
            {
                FirstName = ""
            };

            //Act
            var response = _userController.UpdateUser(userId, updateUserRequest).Result as OkResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region GetUsers

        [Fact(DisplayName = "GetUser Should Return List of Users")]
        public void GetUsers_Should_Return_List_Of_Users()
        {
            //Arrange
            var request = new AdminFilterRequest();

            int totalCount = _usersData.Count();
            var queryResult = _usersData.Take(PagedResourceBase.DefaultPageSize).ToArray();

            _queryServiceMock.Setup(_ => _.Execute(It.IsAny<UserListQuery>(), out totalCount))
                .Returns(queryResult);

            foreach (var partnerUser in queryResult)
            {
                _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                    partnerUser.UserId, It.IsAny<string>())).Returns(Task.FromResult(new HttpServiceResult<bool>
                    {
                        Status = ServiceResultStatus.Success,
                        Model = _usersVerificationStatus.Single(i => i.Item1 == partnerUser.UserId).Item2
                    }));
            }

            //Act
            var response = _userController.GetUserList(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var partnerUserResponse = (AdminPaginatedResponse)response.Value;

            Assert.True(partnerUserResponse.Users.Any());
            Assert.Equal(PagedResourceBase.DefaultPageSize, partnerUserResponse.PageSize);
            Assert.Equal(PagedResourceBase.DefaultPageNumber, partnerUserResponse.Page);
            Assert.Equal(totalCount, partnerUserResponse.TotalCount);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        #endregion
    }
}
