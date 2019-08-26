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
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly IQueryService _queryService;
        private readonly IUserVerificationStatusService _userVerificationStatusService;
        private readonly IMapper _mapper;

        public AdminController(IMapper mapper,
            IAdminService adminService,
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

            var partnerUserListResult = _queryService.Execute(query, out var totalCount);
            var partnerUserList = _mapper.Map<IEnumerable<UserResponse>>(partnerUserListResult).ToArray();

            var partnerUsersPaginatedResponse =
                new AdminPaginatedResponse(totalCount, query.Pagination.pageNumber, query.Pagination.size)
                {
                    Users = partnerUserList
                };

            return Ok(partnerUsersPaginatedResponse);
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
        [HttpPost(Name = "CreatePartnerUser")]
        [ProducesResponseType(typeof(CreateUserResponse), 201)]
        public async Task<IActionResult> CreatePartnerUser([FromBody] CreateUserRequest request)
        {
            var serviceResult = await _adminService.AddUserAsync(request.UserGuid, request.Email);
            if (serviceResult.IsSuccess == false)
            {
                return StatusCode((int)serviceResult.HttpStatusCode, serviceResult.Error);
            }

            var createdPartnerUserResponse = new CreateUserResponse { UserId = serviceResult.Model };

            return CreatedAtRoute("GetPartnerUser",
                new { partnerUserId = createdPartnerUserResponse.UserId }, createdPartnerUserResponse);
        }

        /// <summary>
        /// Updates a User by UserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to update a User
        /// </remarks>
        /// <param name="partnerUserId"></param>
        /// <param  name="request">Details required for update of User</param>
        /// <returns>Returns OK on successful update of User</returns>
        /// <response code="200">When a new User is updated</response>
        /// <response code="404">User not found by partnerUserId</response>
        [HttpPut("{partnerUserId:guid}", Name = "UpdatePartnerUser")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdatePartnerUser(Guid partnerUserId, [FromBody] UpdateUserRequest request)
        {
            var validateRequestResult = request.ValidateAllEditablePropertiesSet();
            if (validateRequestResult.IsSuccess == false)
            {
                return StatusCode((int)validateRequestResult.HttpStatusCode, validateRequestResult.Error);
            }

            var partnerServiceResult = await _adminService.GetUserAsync(partnerUserId);

            if (partnerServiceResult.IsSuccess == false)
            {
                return StatusCode((int)partnerServiceResult.HttpStatusCode, partnerServiceResult.Error);
            }

            var partnerUser = partnerServiceResult.Model;

            await _adminService.UpdateUserAsync(partnerUser);

            return Ok();
        }

        /// <summary>
        /// Gets a User by UserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrive a User by UserId
        /// </remarks>
        /// <param name="partnerUserId"></param>
        /// <returns>Returns the requested User</returns>
        /// <response code="200">When a User is returned</response>
        /// <response code="404">User not found by partnerUserId</response>
        [HttpGet("{partnerUserId:guid}", Name = "GetPartnerUser")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public async Task<IActionResult> GetPartnerUser(Guid partnerUserId)
        {
            var partnerUserResult = await _adminService.GetUserAsync(partnerUserId);

            if (partnerUserResult.IsSuccess == false)
            {
                return StatusCode((int)partnerUserResult.HttpStatusCode, partnerUserResult.Error);
            }

            var partnerUserResponse = _mapper.Map<UserResponse>(partnerUserResult.Model);

            return Ok(partnerUserResponse);
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