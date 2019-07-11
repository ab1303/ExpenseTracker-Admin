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
    public class PartnerUsersController : ControllerBase
    {
        private readonly IPartnerUserService _partnerUserService;
        private readonly IQueryService _queryService;
        private readonly IUserVerificationStatusService _userVerificationStatusService;
        private readonly IMapper _mapper;

        public PartnerUsersController(IMapper mapper,
            IPartnerUserService partnerUserService,
            IUserVerificationStatusService userVerificationStatusService,
            IQueryService queryService
            )
        {
            _mapper = mapper;
            _partnerUserService = partnerUserService;
            _queryService = queryService;
            _userVerificationStatusService = userVerificationStatusService;
        }

        /// <summary>
        /// Get a list of all partner User resources filtered by partnerAppId and ofxUserGuid
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Produces a response comprising a list of PartnerUser resources</returns>
        /// <response code="200">When List of PartnerUser is returned</response>
        [HttpGet("", Name = "GetPartnerUsers")]
        [ProducesResponseType(typeof(PartnerUsersPaginatedResponse), 200)]
        public async Task<IActionResult> GetPartnerUserList([FromQuery] PartnerUserFilterRequest request)
        {
            var query = new PartnerUserListQuery(
                (request.OfxUserGuid, request.PartnerAppId, request.PartnerUserId, request.BeneficiaryId),
                (request.PageNumber, request.PageSize)
            )
            {
                ReturnAllResults = request.IsReadyToDeal.HasValue
            };

            var partnerUserListResult = _queryService.Execute(query, out var totalCount);
            var partnerUserList = _mapper.Map<IEnumerable<PartnerUserResponse>>(partnerUserListResult).ToArray();

            foreach (var partnerUser in partnerUserList)
            {
                var isUserReadyToDealResult = await _userVerificationStatusService.GetIsUserReadyToDealAsync(partnerUser.OfxUserGuid, GetBearerToken());
                if (!isUserReadyToDealResult.IsSuccess)
                {
                    return StatusCode((int)isUserReadyToDealResult.HttpStatusCode, isUserReadyToDealResult.Error);
                }
                partnerUser.IsReadyToDeal = isUserReadyToDealResult.Model;
            }

            if (request.IsReadyToDeal.HasValue)
            {
                totalCount = partnerUserList.Count(pu => pu.IsReadyToDeal == request.IsReadyToDeal.Value);

                partnerUserList = partnerUserList
                    .Where(pu => pu.IsReadyToDeal == request.IsReadyToDeal.Value)
                                    .Skip(query.Pagination.size * (query.Pagination.pageNumber - 1))
                                    .Take(query.Pagination.size)
                                    .ToArray();
            }

            var partnerUsersPaginatedResponse =
                new PartnerUsersPaginatedResponse(totalCount, query.Pagination.pageNumber, query.Pagination.size)
                {
                    Users = partnerUserList
                };

            return Ok(partnerUsersPaginatedResponse);
        }

        /// <summary>
        /// Creates a new PartnerUser relationship and returns PartnerUserId GUID
        /// </summary>
        /// <remarks>
        /// Use this endpoint to create a PartnerUser
        /// </remarks>
        /// <param  name="request">Details required for creation of PartnerUser</param>
        /// <returns>Returns PartnerUserId Guid on successful creation of PartnerUser</returns>
        /// <response code="201">When a new PartnerUser is created</response>
        /// <response code="400">If OfxUserGuid is empty, or PartnerAppId is empty</response>
        /// <response code="409">OfxUserGuid + PartnerAppId already exists in the system</response>
        [HttpPost(Name = "CreatePartnerUser")]
        [ProducesResponseType(typeof(CreatePartnerUserResponse), 201)]
        public async Task<IActionResult> CreatePartnerUser([FromBody] CreatePartnerUserRequest request)
        {
            var serviceResult = await _partnerUserService.AddPartnerUserAsync(request.OfxUserGuid, request.PartnerAppId);
            if (serviceResult.IsSuccess == false)
            {
                return StatusCode((int)serviceResult.HttpStatusCode, serviceResult.Error);
            }

            var createdPartnerUserResponse = new CreatePartnerUserResponse { PartnerUserId = serviceResult.Model };

            return CreatedAtRoute("GetPartnerUser",
                new { partnerUserId = createdPartnerUserResponse.PartnerUserId }, createdPartnerUserResponse);
        }

        /// <summary>
        /// Updates a PartnerUser by PartnerUserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to update a PartnerUser
        /// </remarks>
        /// <param name="partnerUserId"></param>
        /// <param  name="request">Details required for update of PartnerUser</param>
        /// <returns>Returns OK on successful update of PartnerUser</returns>
        /// <response code="200">When a new PartnerUser is updated</response>
        /// <response code="404">PartnerUser not found by partnerUserId</response>
        [HttpPut("{partnerUserId:guid}", Name = "UpdatePartnerUser")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> UpdatePartnerUser(Guid partnerUserId, [FromBody] UpdatePartnerUserRequest request)
        {
            var validateRequestResult = request.ValidateAllEditablePropertiesSet();
            if (validateRequestResult.IsSuccess == false)
            {
                return StatusCode((int)validateRequestResult.HttpStatusCode, validateRequestResult.Error);
            }

            var partnerServiceResult = await _partnerUserService.GetPartnerUserAsync(partnerUserId);

            if (partnerServiceResult.IsSuccess == false)
            {
                return StatusCode((int)partnerServiceResult.HttpStatusCode, partnerServiceResult.Error);
            }

            var partnerUser = partnerServiceResult.Model;
            partnerUser.BeneficiaryId = request.BeneficiaryId;

            await _partnerUserService.UpdatePartnerUserAsync(partnerUser);

            return Ok();
        }

        /// <summary>
        /// Gets a PartnerUser by PartnerUserId
        /// </summary>
        /// <remarks>
        /// Use this endpoint to retrive a PartnerUser by PartnerUserId
        /// </remarks>
        /// <param name="partnerUserId"></param>
        /// <returns>Returns the requested PartnerUser</returns>
        /// <response code="200">When a PartnerUser is returned</response>
        /// <response code="404">PartnerUser not found by partnerUserId</response>
        [HttpGet("{partnerUserId:guid}", Name = "GetPartnerUser")]
        [ProducesResponseType(typeof(PartnerUserResponse), 200)]
        public async Task<IActionResult> GetPartnerUser(Guid partnerUserId)
        {
            var partnerUserResult = await _partnerUserService.GetPartnerUserAsync(partnerUserId);

            if (partnerUserResult.IsSuccess == false)
            {
                return StatusCode((int)partnerUserResult.HttpStatusCode, partnerUserResult.Error);
            }

            var partnerUser = partnerUserResult.Model;

            var isUserReadyToDealResult = await _userVerificationStatusService.GetIsUserReadyToDealAsync(partnerUser.OfxUserGuid, GetBearerToken());

            if (!isUserReadyToDealResult.IsSuccess)
            {
                return StatusCode((int)isUserReadyToDealResult.HttpStatusCode, isUserReadyToDealResult.Error);
            }

            var partnerUserResponse = _mapper.Map<PartnerUserResponse>(partnerUser);
            partnerUserResponse.IsReadyToDeal = isUserReadyToDealResult.Model;

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