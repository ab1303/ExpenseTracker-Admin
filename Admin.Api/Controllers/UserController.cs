using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Admin.Api.Requests;
using Admin.Api.Responses;
using Admin.Common;
using Admin.Services.interfaces;
using Admin.Services.Queries;

namespace Admin.Api.Controllers
{
    [Route("api/" + Constants.ApiVersion.V1 + "/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _adminService;
        private readonly IQueryService _queryService;
        private readonly IUserVerificationStatusService _userVerificationStatusService;
        private readonly IMapper _mapper;

        public UserController(IMapper mapper,
            IUserService adminService,
            IUserVerificationStatusService userVerificationStatusService,
            IQueryService queryService
            )
        {
            _mapper = mapper;
            _adminService = adminService;
            _queryService = queryService;
            _userVerificationStatusService = userVerificationStatusService;
        }

        /// <summary>
        /// Get a list of all partner User resources filtered by partnerAppId and ofxUserGuid
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Produces a response comprising a list of User resources</returns>
        /// <response code="200">When List of User is returned</response>
        [HttpGet("", Name = "GetUsers")]
        [ProducesResponseType(typeof(AdminPaginatedResponse), 200)]
        public async Task<IActionResult> GetUserList([FromQuery] AdminFilterRequest request)
        {
            var query = new UserListQuery(
                (request.Email, request.FirstName),
                (request.PageNumber, request.PageSize)
            )
            {
                ReturnAllResults = false
            };

            var userListResult = _queryService.Execute(query, out var totalCount);
            var userList = _mapper.Map<IEnumerable<UserResponse>>(userListResult).ToArray();

            var usersPaginatedResponse =
                new AdminPaginatedResponse(totalCount, query.Pagination.pageNumber, query.Pagination.size)
                {
                    Users = userList
                };

            return Ok(usersPaginatedResponse);
        }

        /// <summary>
        /// Creates a new User relationship and returns UserId GUID
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a User
        /// </remarks>
        /// <param  name="request">Details required for creation of User</param>
        /// <returns>Returns UserId Guid on successful creation of User</returns>
        /// <response code="201">When a new User is created</response>
        /// <response code="400">If UserGuid is empty, or FirstName is empty</response>
        /// <response code="409">UserGuid + FirstName already exists in the system</response>
        [HttpPost(Name = "CreateUser")]
        [ProducesResponseType(typeof(CreateUserResponse), 201)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var serviceResult = await _adminService.AddUserAsync(request.UserGuid, request.Email);
            if (serviceResult.IsSuccess == false)
            {
                return StatusCode((int)serviceResult.HttpStatusCode, serviceResult.Error);
            }

            var createdUserResponse = new CreateUserResponse { UserId = serviceResult.Model };

            return CreatedAtRoute("GetUser",
                new { userId = createdUserResponse.UserId }, createdUserResponse);
        }

        /// <summary>
        /// Updates a User by UserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to update a User
        /// </remarks>
        /// <param name="userId"></param>
        /// <param  name="request">Details required for update of User</param>
        /// <returns>Returns OK on successful update of User</returns>
        /// <response code="200">When a new User is updated</response>
        /// <response code="404">User not found by userId</response>
        [HttpPut("{userId:guid}", Name = "UpdateUser")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserRequest request)
        {
            var validateRequestResult = request.ValidateAllEditablePropertiesSet();
            if (validateRequestResult.IsSuccess == false)
            {
                return StatusCode((int)validateRequestResult.HttpStatusCode, validateRequestResult.Error);
            }

            var partnerServiceResult = await _adminService.GetUserAsync(userId);

            if (partnerServiceResult.IsSuccess == false)
            {
                return StatusCode((int)partnerServiceResult.HttpStatusCode, partnerServiceResult.Error);
            }

            var user = partnerServiceResult.Model;

            await _adminService.UpdateUserAsync(user);

            return Ok();
        }

        /// <summary>
        /// Gets a User by UserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrive a User by UserId
        /// </remarks>
        /// <param name="userId"></param>
        /// <returns>Returns the requested User</returns>
        /// <response code="200">When a User is returned</response>
        /// <response code="404">User not found by userId</response>
        [HttpGet("{userId:guid}", Name = "GetUser")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> GetUser(Guid userId)
        {
            var userResult = await _adminService.GetUserAsync(userId);

            if (userResult.IsSuccess == false)
            {
                return StatusCode((int)userResult.HttpStatusCode, userResult.Error);
            }

            var userResponse = _mapper.Map<UserResponse>(userResult.Model);

            return Ok(userResponse);
        }

        private string GetBearerToken()
        {
            var authorizationHeaderValue = Request.Headers[Constants.AuthorizationHeaderName];
            var token = "";
            if (!string.IsNullOrWhiteSpace(authorizationHeaderValue))
            {
                token = authorizationHeaderValue.ToString().Replace("Bearer", "").Trim();
            }
            return token;
        }
    }
}