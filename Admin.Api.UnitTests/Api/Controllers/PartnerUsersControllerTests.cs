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
    public class PartnerUsersControllerTests
    {

        private readonly IEnumerable<Admin.Domain.Model.PartnerUser> _partnerUsersData;
        private readonly IEnumerable<(Guid, bool)> _usersVerificationStatus;
        private readonly Mock<IPartnerUserService> _partnerUserServiceMock;
        private readonly Mock<IUserVerificationStatusService> _userVerificationStatusMock;
        private readonly Mock<IQueryService> _queryServiceMock;
        private readonly PartnerUsersController _partnerUsersController;

        public PartnerUsersControllerTests()
        {

            var defaultHttpContext = new DefaultHttpContext();
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultHttpContext);
            var mockedStore = new MockedDataStore();
            _partnerUsersData = mockedStore.PartnerUsersData();
            _usersVerificationStatus = mockedStore.UsersBslStatusData();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PartnerUserProfile());
            });
            var mapper = mockMapper.CreateMapper();

            _partnerUserServiceMock = new Mock<IPartnerUserService>();
            _userVerificationStatusMock = new Mock<IUserVerificationStatusService>();
            _queryServiceMock = new Mock<IQueryService>();

            _partnerUsersController = new PartnerUsersController(mapper,
                _partnerUserServiceMock.Object, _userVerificationStatusMock.Object, _queryServiceMock.Object)
            {
                ControllerContext = new ControllerContext { HttpContext = defaultHttpContext }
            };
        }

        #region CreatePartnerUser

        [Fact(DisplayName = "CreatePartnerUser Should Return UnProcessableEntity StatusCode On PartnerUserService Failure")]
        public void CreatePartnerUser_Should_Return_UnProcessableEntity_StatusCode_On_PartnerUserService_Failure()
        {
            //Arrange
            var request = new CreatePartnerUserRequest
            {
                OfxUserGuid = Guid.NewGuid(),
                PartnerAppId = Guid.NewGuid()
            };

            var failResult = new HttpServiceResult<Guid>
            {
                HttpStatusCode = HttpStatusCode.UnprocessableEntity,
                Error = new Error { ErrorCode = ErrorCodes.DuplicatePartnerUserInsert, SystemMessage = "Failed!" },
                Status = ServiceResultStatus.Failure
            };

            _partnerUserServiceMock.Setup(svc => svc.AddPartnerUserAsync(request.OfxUserGuid, request.PartnerAppId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _partnerUsersController.CreatePartnerUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "CreatePartnerUser Should Return Created StatusCode And PartnerUserId On PartnerUserService Success")]
        public void CreatePartnerUser_Should_Return_Created_StatusCode_And_PartnerUserId_On_PartnerUserService_Success()
        {
            //Arrange
            var request = new CreatePartnerUserRequest
            {
                OfxUserGuid = Guid.NewGuid(),
                PartnerAppId = Guid.NewGuid()
            };

            var newPartnerUserId = Guid.NewGuid();
            var successResult = new HttpServiceResult<Guid>
            {
                Model = newPartnerUserId,
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.AddPartnerUserAsync(request.OfxUserGuid, request.PartnerAppId))
                .Returns(Task.FromResult(successResult));

            //Act
            var response = _partnerUsersController.CreatePartnerUser(request).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(newPartnerUserId, ((CreatePartnerUserResponse)response.Value).PartnerUserId);
        }

        #endregion

        #region GetPartnerUser

        [Fact(DisplayName = "GetPartnerUser Should Return Not Found StatusCode On PartnerUser Not Found")]
        public void GetPartnerUser_Should_Return_Not_Found_StatusCode_On_PartnerUser_Not_Found()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.PartnerUserNotFound, SystemMessage = "Not found!" }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetPartnerUserAsync(partnerUserId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _partnerUsersController.GetPartnerUser(partnerUserId).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.NotFound, response.StatusCode);
            Assert.Equal(failResult.Error, response.Value);
        }

        [Fact(DisplayName = "GetPartnerUser Should Return Partner User If Found")]
        public void GetPartnerUser_Should_Return_PartnerUser_If_Found()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var getPartnerUserSuccessResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.PartnerUser { PartnerUserId = partnerUserId, OfxUserGuid = Guid.NewGuid(), PartnerAppId = Guid.NewGuid() }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetPartnerUserAsync(partnerUserId))
                .Returns(Task.FromResult(getPartnerUserSuccessResult));

            var getIsUserReadyToDealSuccessResult = new HttpServiceResult<bool>
            {
                Status = ServiceResultStatus.Success,
                Model = true
            };

            _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.FromResult(getIsUserReadyToDealSuccessResult));

            //Act
            var response = _partnerUsersController.GetPartnerUser(partnerUserId).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var getPartnerUserResponse = response.Value as PartnerUserResponse;

            Assert.NotNull(getPartnerUserResponse);
            Assert.Equal(getPartnerUserSuccessResult.Model.PartnerUserId, getPartnerUserResponse.PartnerUserId);
            Assert.Equal(getPartnerUserSuccessResult.Model.OfxUserGuid, getPartnerUserResponse.OfxUserGuid);
            Assert.Equal(getPartnerUserSuccessResult.Model.PartnerAppId, getPartnerUserResponse.PartnerAppId);
            Assert.Equal(getIsUserReadyToDealSuccessResult.Model, getPartnerUserResponse.IsReadyToDeal);
        }

        #endregion

        #region UpdatePartnerUser

        [Fact(DisplayName = "UpdatePartnerUser Should Return Not Found StatusCode On PartnerUser Not Found")]
        public void UpdatePartnerUser_Should_Return_Not_Found_StatusCode_On_PartnerUser_Not_Found()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Status = ServiceResultStatus.Failure,
                Error = new Error { ErrorCode = ErrorCodes.PartnerUserNotFound, SystemMessage = "Not found!" }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetPartnerUserAsync(partnerUserId))
                .Returns(Task.FromResult(failResult));

            //Act
            var response = _partnerUsersController.UpdatePartnerUser(partnerUserId, new UpdatePartnerUserRequest
            {
                BeneficiaryId = Guid.NewGuid()
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
            var partnerUserId = Guid.NewGuid();

            var failResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Status = ServiceResultStatus.Failure,
                Error = new Error
                {
                    ErrorCode = ErrorCodes.RequestPropertyNotSet,
                    SystemMessage = "The BeneficiaryId field must be set to null or a Guid."
                }
            };

            //Act
            var response = _partnerUsersController.UpdatePartnerUser(partnerUserId, new UpdatePartnerUserRequest()).Result as ObjectResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(failResult.Error.ErrorCode, ((Error)response.Value).ErrorCode);
            Assert.Equal(failResult.Error.SystemMessage, ((Error)response.Value).SystemMessage);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return Ok Status Code On Success Update with BeneficiaryId Guid")]
        public void UpdatePartnerUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_Guid()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var successGetPartnerUserResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.PartnerUser
                {
                    PartnerUserId = partnerUserId,
                    OfxUserGuid = Guid.NewGuid(),
                    PartnerAppId = Guid.NewGuid()
                }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetPartnerUserAsync(partnerUserId))
                .Returns(Task.FromResult(successGetPartnerUserResult));

            var successUpdatePartnerUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.UpdatePartnerUserAsync(successGetPartnerUserResult.Model))
                .Returns(Task.FromResult(successUpdatePartnerUserResult));

            var updatePartnerUserRequest = new UpdatePartnerUserRequest
            {
                BeneficiaryId = new Guid()
            };

            //Act
            var response = _partnerUsersController.UpdatePartnerUser(partnerUserId, updatePartnerUserRequest).Result as OkResult;

            //Assert
            Assert.NotNull(response);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "UpdatePartnerUser Should Return Ok Status Code On Success Update with BeneficiaryId To Null")]
        public void UpdatePartnerUser_Should_Return_Ok_Status_Code_On_Success_Update_With_BeneficiaryId_To_Null()
        {
            //Arrange
            var partnerUserId = Guid.NewGuid();

            var successGetPartnerUserResult = new HttpServiceResult<Admin.Domain.Model.PartnerUser>
            {
                Status = ServiceResultStatus.Success,
                Model = new Admin.Domain.Model.PartnerUser
                {
                    PartnerUserId = partnerUserId,
                    OfxUserGuid = Guid.NewGuid(),
                    PartnerAppId = Guid.NewGuid()
                }
            };

            _partnerUserServiceMock.Setup(svc => svc.GetPartnerUserAsync(partnerUserId))
                .Returns(Task.FromResult(successGetPartnerUserResult));

            var successUpdatePartnerUserResult = new ServiceResult
            {
                Status = ServiceResultStatus.Success
            };

            _partnerUserServiceMock.Setup(svc => svc.UpdatePartnerUserAsync(successGetPartnerUserResult.Model))
                .Returns(Task.FromResult(successUpdatePartnerUserResult));

            var updatePartnerUserRequest = new UpdatePartnerUserRequest
            {
                BeneficiaryId = null
            };

            //Act
            var response = _partnerUsersController.UpdatePartnerUser(partnerUserId, updatePartnerUserRequest).Result as OkResult;

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
            var request = new PartnerUserFilterRequest();

            int totalCount = _partnerUsersData.Count();
            var queryResult = _partnerUsersData.Take(PagedResourceBase.DefaultPageSize).ToArray();

            _queryServiceMock.Setup(_ => _.Execute(It.IsAny<PartnerUserListQuery>(), out totalCount))
                .Returns(queryResult);

            foreach (var partnerUser in queryResult)
            {
                _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                    partnerUser.OfxUserGuid, It.IsAny<string>())).Returns(Task.FromResult(new HttpServiceResult<bool>
                    {
                        Status = ServiceResultStatus.Success,
                        Model = _usersVerificationStatus.Single(i => i.Item1 == partnerUser.OfxUserGuid).Item2
                    }));
            }

            //Act
            var response = _partnerUsersController.GetPartnerUserList(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var partnerUserResponse = (PartnerUsersPaginatedResponse)response.Value;

            Assert.True(partnerUserResponse.Users.Any());
            Assert.Equal(PagedResourceBase.DefaultPageSize, partnerUserResponse.PageSize);
            Assert.Equal(PagedResourceBase.DefaultPageNumber, partnerUserResponse.Page);
            Assert.Equal(totalCount, partnerUserResponse.TotalCount);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GetPartnerUser Should Return List of Partner Users for Active Users")]
        public void GetPartnerUsers_Should_Return_List_Of_PartnerUsers_For_Active_Users()
        {
            //Arrange
            var request = new PartnerUserFilterRequest { IsReadyToDeal = true };

            int totalCount;
            var queryResult = _partnerUsersData.Take(PagedResourceBase.DefaultPageSize).ToArray();

            _queryServiceMock.Setup(_ => _.Execute(It.IsAny<PartnerUserListQuery>(), out totalCount))
                .Returns(queryResult);

            foreach (var partnerUser in queryResult)
            {
                _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                    partnerUser.OfxUserGuid, It.IsAny<string>())).Returns(Task.FromResult(new HttpServiceResult<bool>
                    {
                        Status = ServiceResultStatus.Success,
                        Model = _usersVerificationStatus.Single(i => i.Item1 == partnerUser.OfxUserGuid).Item2
                    }));
            }

            //Act
            var response = _partnerUsersController.GetPartnerUserList(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var partnerUserResponse = (PartnerUsersPaginatedResponse)response.Value;

            Assert.True(partnerUserResponse.Users.All(pu => pu.IsReadyToDeal));
            Assert.Equal(PagedResourceBase.DefaultPageSize, partnerUserResponse.PageSize);
            Assert.Equal(PagedResourceBase.DefaultPageNumber, partnerUserResponse.Page);
            Assert.Equal(partnerUserResponse.Users.Count(), partnerUserResponse.TotalCount);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "GetPartnerUser Should Return List of Partner Users for InActive Users")]
        public void GetPartnerUsers_Should_Return_List_Of_PartnerUsers_For_InActive_Users()
        {
            //Arrange
            var request = new PartnerUserFilterRequest { IsReadyToDeal = false };

            int totalCount;
            var queryResult = _partnerUsersData.Take(PagedResourceBase.DefaultPageSize).ToArray();

            _queryServiceMock.Setup(_ => _.Execute(It.IsAny<PartnerUserListQuery>(), out totalCount))
                .Returns(queryResult);

            foreach (var partnerUser in queryResult)
            {
                _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                    partnerUser.OfxUserGuid, It.IsAny<string>())).Returns(Task.FromResult(new HttpServiceResult<bool>
                    {
                        Status = ServiceResultStatus.Success,
                        Model = _usersVerificationStatus.Single(i => i.Item1 == partnerUser.OfxUserGuid).Item2
                    }));
            }

            //Act
            var response = _partnerUsersController.GetPartnerUserList(request).Result as OkObjectResult;

            //Assert
            Assert.NotNull(response);
            var partnerUserResponse = (PartnerUsersPaginatedResponse)response.Value;

            Assert.True(partnerUserResponse.Users.All(pu => pu.IsReadyToDeal == false));
            Assert.Equal(PagedResourceBase.DefaultPageSize, partnerUserResponse.PageSize);
            Assert.Equal(PagedResourceBase.DefaultPageNumber, partnerUserResponse.Page);
            Assert.Equal(partnerUserResponse.Users.Count(), partnerUserResponse.TotalCount);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);

        }

        [Fact(DisplayName = "GetPartnerUser Should Return List of Partner Users for given User")]
        public void GetPartnerUsers_Should_Return_List_Of_PartnerUsers_For_Given_User()
        {
            //Arrange
            var userGuid = Guid.Parse("c0e818ae-8131-4f2b-a3c9-ef843e8b7bcd");
            var request = new PartnerUserFilterRequest { OfxUserGuid = userGuid };
            var filteredResult = _partnerUsersData.Where(d => d.OfxUserGuid == userGuid).ToArray();
            int totalCount = _partnerUsersData.Count();
            var queryResult = filteredResult.Take(PagedResourceBase.DefaultPageSize).ToArray();

            _queryServiceMock.Setup(_ => _.Execute(It.IsAny<PartnerUserListQuery>(), out totalCount))
                .Returns(queryResult);

            foreach (var partnerUser in queryResult)
            {
                _userVerificationStatusMock.Setup(svc => svc.GetIsUserReadyToDealAsync(
                    partnerUser.OfxUserGuid, It.IsAny<string>())).Returns(Task.FromResult(new HttpServiceResult<bool>
                    {
                        Status = ServiceResultStatus.Success,
                        Model = _usersVerificationStatus.Single(i => i.Item1 == partnerUser.OfxUserGuid).Item2
                    }));
            }

            //Act
            var response = _partnerUsersController.GetPartnerUserList(request).Result as OkObjectResult;

            //Assert           
            Assert.NotNull(response);
            var partnerUserResponse = (PartnerUsersPaginatedResponse)response.Value;

            Assert.True(partnerUserResponse.Users.All(pu => pu.OfxUserGuid == userGuid));
            Assert.Equal(PagedResourceBase.DefaultPageSize, partnerUserResponse.PageSize);
            Assert.Equal(PagedResourceBase.DefaultPageNumber, partnerUserResponse.Page);
            Assert.Equal(totalCount, partnerUserResponse.TotalCount);
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
        }

        #endregion
    }
}
