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

        private readonly IEnumerable<Admin.Domain.Model.User> _partnerUsersData;
        private readonly IEnumerable<(Guid, bool)> _usersVerificationStatus;
        private readonly Mock<IAdminService> _partnerUserServiceMock;
        private readonly Mock<IUserVerificationStatusService> _userVerificationStatusMock;
        private readonly Mock<IQueryService> _queryServiceMock;
        private readonly AdminController _adminController;

        public UsersControllerTests()
        {

            var defaultHttpContext = new DefaultHttpContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultHttpContext);
            var mockedStore = new MockedDataStore();
            _partnerUsersData = mockedStore.UsersData();
            _usersVerificationStatus = mockedStore.UsersBslStatusData();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _partnerUserServiceMock = new Mock<IAdminService>();
            _userVerificationStatusMock = new Mock<IUserVerificationStatusService>();
            _queryServiceMock = new Mock<IQueryService>();

            _adminController = new AdminController(mapper,
                _partnerUserServiceMock.Object, _userVerificationStatusMock.Object, _queryServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = defaultHttpContext }
            };
        }

        #region CreatePartnerUser

        [Fact(DisplayName = "CreatePartnerUser Should Return UnProcessableEntity StatusCode On AdminService Failure")]
        public void CreatePartnerUser_Should_Return_UnProcessableEntity_StatusCode_On_PartnerUserService_Failure()
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

            _partnerUserServiceMock.Setup(svc => svc.AddUserAsync(request.UserGuid, request.Email))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _adminController.CreatePartnerUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Return Created StatusCode And UserId On AdminService Success")]
        public void CreatePartnerUser_Should_Return_Created_StatusCode_And_PartnerUserId_On_PartnerUserService_Success()
        {
            //Arrange
            var request = new CreateUserRequest
            {
                UserGuid = Guid.NewGuid(),
                Email = "email@test.com"
            };

            var newPartnerUserId = Guid.NewGuid();
            var successResult = new HttpServiceResult<Guid>
            {
                Model = newPartnerUserId,
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.AddUserAsync(request.UserGuid, request.Email))
                .Returns(Task.FromResult(successResult));

            //Act
            var response = _adminController.CreatePartnerUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(newPartnerUserId, ((CreateUserResponse)response.Value).UserId);
        }

        #endregion

        #region GetPartnerUser

        [Fact(DisplayName = "GetPartnerUser Should Return Not Found StatusCode On User Not Found")]
        public void GetPartnerUser_Should_Return_Not_Found_StatusCode_On_PartnerUser_Not_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Domain.Model.User>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.UserNotFound, SystemMessage = "Not found!" }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _adminController.GetPartnerUser(userId).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "GetPartnerUser Should Return Partner User If Found")]
        public void GetPartnerUser_Should_Return_PartnerUser_If_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var getPartnerUserSuccessResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.User { UserId = userId }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(getPartnerUserSuccessResult));

            var getIsUserReadyToDealSuccessResult = new HttpServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Model = true
            };

            _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(getIsUserReadyToDealSuccessResult));

            //Act
            var response = _adminController.GetPartnerUser(userId).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var getPartnerUserResponse = response.Value as UserResponse;

            Assert.NotNull(getPartnerUserResponse);
            Assert.Equal(getPartnerUserSuccessResult.Model.UserId, getPartnerUserResponse.UserId);
            Assert.Equal(getPartnerUserSuccessResult.Model.Email, getPartnerUserResponse.Email);
            Assert.Equal(getPartnerUserSuccessResult.Model.FirstName, getPartnerUserResponse.FirstName);
        }

        #endregion

        #region UpdatePartnerUser

        [Fact(DisplayName = "UpdatePartnerUser Should Return Not Found StatusCode On User Not Found")]
        public void UpdatePartnerUser_Should_Return_Not_Found_StatusCode_On_PartnerUser_Not_Found()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.UserNotFound, SystemMessage = "Not found!" }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _adminController.UpdatePartnerUser(userId, new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            }).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return Bad Request StatusCode On Beneficiary Id Property Not Set")]
        public void UpdatePartnerUser_Should_Return_Bad_Request_StatusCode_On_BeneficiaryId_Property_Not_Set()
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
            var response = _adminController.UpdatePartnerUser(userId, new UpdateUserRequest()).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(failResult.Error.ErrorCode, ((Error)response.Value).ErrorCode);
            Assert.Equal(failResult.Error.SystemMessage, ((Error)response.Value).SystemMessage);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return Ok Status Code On Success Update with Email Guid")]
        public void UpdatePartnerUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_Guid()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var successGetPartnerUserResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.User
                {
                    UserId = userId,
                }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(successGetPartnerUserResult));

            var successUpdatePartnerUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.UpdateUserAsync(successGetPartnerUserResult.Model))
                .Returns(Task.FromResult(successUpdatePartnerUserResult));

            var updatePartnerUserRequest = new UpdateUserRequest
            {
                FirstName = "Updated FirstName"
            };

            //Act
            var response = _adminController.UpdatePartnerUser(userId, updatePartnerUserRequest).Result as OkResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return Ok Status Code On Success Update with Email To Null")]
        public void UpdatePartnerUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_To_Null()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var successGetPartnerUserResult = new HttpServiceResult<Admin.Domain.Model.User>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.User
                {
                    UserId = userId,
                }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetUserAsync(userId))
                .Returns(Task.FromResult(successGetPartnerUserResult));

            var successUpdatePartnerUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.UpdateUserAsync(successGetPartnerUserResult.Model))
                .Returns(Task.FromResult(successUpdatePartnerUserResult));

            var updatePartnerUserRequest = new UpdateUserRequest
            {
                FirstName = ""
            };

            //Act
            var response = _adminController.UpdatePartnerUser(userId, updatePartnerUserRequest).Result as OkResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        #endregion

        #region GetPartnerUsers

        [Fact(DisplayName = "GetPartnerUser Should Return List of Partner Users")]
        public void GetPartnerUsers_Should_Return_List_Of_PartnerUsers()
        {
            //Arrange
            var request = new AdminFilterRequest();

            int totalCount = _partnerUsersData.Count();
            var queryResult = _partnerUsersData.Take(PagedResourceBase.DefaultPageSize).ToArray();

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
            var response = _adminController.GetUserList(request).Result as OkObjectResult;

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
