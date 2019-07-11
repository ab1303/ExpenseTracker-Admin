using System;
using System.Collections.Generic;
using System.Linq;
using Admin.Api.UnitTests.Repositories;
using Admin.Common.Models;
using Admin.Repositories;
using Admin.Services;
using Admin.Services.interfaces;
using Admin.Services.Queries;
using Moq;
using Xunit;

namespace Admin.Api.UnitTests.Queries
{
    public class PartnerUserQueryTests
    {
        private readonly IQueryService _queryService;
        private readonly IEnumerable<Admin.Domain.Model.PartnerUser> _partnerUsersData;

        public PartnerUserQueryTests()
        {
            _partnerUsersData = new MockedDataStore().PartnerUsersData();

            var partnerUserRepositoryMock = new Mock<IPartnerUserRepository>();
            partnerUserRepositoryMock
                .Setup(_ => _.GetAll()).Returns(_partnerUsersData.AsQueryable());

            _queryService = new QueryService(new MockRepositories(partnerUserRepositoryMock.Object));
        }

        [Fact(DisplayName = "PartnerUserListQuery filters results using defaults when no query parameters are provided")]
        public void PartnerUserListQuery_returns_all_results_using_defaults_when_no_query_parameters_are_provided()
        {
            //Arrange
            var query = new PartnerUserListQuery(
                (ofxUserGuid: null, partnerAppId: null, partnerUserId: null, beneficiaryId: null)
            );

            //Act
            var results = _queryService.Execute(query, out var totalCount);

            //Assert
            Assert.Equal(_partnerUsersData.Count(), totalCount);
            Assert.Equal(results.Count(), PagedResourceBase.DefaultPageSize);
        }

        [Fact(DisplayName = "PartnerUserListQuery filters results by pageSize given pageSize")]
        public void PartnerUserListQuery_filters_results_by_pageSize_given_pageSize()
        {
            //Arrange
            var pageSize = PagedResourceBase.DefaultPageSize + 3;
            var query = new PartnerUserListQuery(
                (ofxUserGuid: null, partnerAppId: null, partnerUserId: null, beneficiaryId: null),
                (pageNumber: 1, size: pageSize)
            );

            //Act
            var results = _queryService.Execute(query, out var _);

            //Assert
            var partnerUsers = results as Admin.Domain.Model.PartnerUser[] ?? results.ToArray();
            Assert.Equal(pageSize, partnerUsers.Length);
            Assert.True(partnerUsers.Length > PagedResourceBase.DefaultPageSize);
        }

        [Fact(DisplayName = "PartnerUserListQuery filters results by maxPageSize given pageSize greater than maxPageSize")]
        public void PartnerUserListQuery_filters_results_by_maxPageSize_given_pageSize_greater_than_maxPageSize()
        {
            //Arrange
            var pageSize = PagedResourceBase.MaxPageSize + 3;
            var query = new PartnerUserListQuery(
                (ofxUserGuid: null, partnerAppId: null, partnerUserId: null, beneficiaryId: null),
                (pageNumber: 1, size: pageSize)
            );

            //Act
            var results = _queryService.Execute(query, out var _);

            //Assert
            var partnerUsers = results as Admin.Domain.Model.PartnerUser[] ?? results.ToArray();
            Assert.True(partnerUsers.Length <= PagedResourceBase.MaxPageSize);
            Assert.True(partnerUsers.Length < pageSize);
        }

        [Fact(DisplayName = "PartnerUserListQuery filters results when query parameters are provided")]
        public void PartnerUserListQuery_filters_results_when_query_parameters_are_provided()
        {
            //Arrange
            var ofxUserGuid = Guid.Parse("ae5e939c-1639-49b2-be1f-ac2d8f588a8f");
            var partnerAppId = Guid.Parse("6cc17482-218a-4bdf-94df-83cf4fda7888");
            var query = new PartnerUserListQuery(
                (ofxUserGuid, partnerAppId, null, null)
            );

            //Act
            _queryService.Execute(query, out var totalCount);

            //Assert
            Assert.True(totalCount < _partnerUsersData.Count());
        }

        [Fact(DisplayName = "PartnerUserListQuery does not perform paging when ReturnAllResults is true")]
        public void PartnerUserListQuery_does_not_perform_paging_when_ReturnAllResults_is_true()
        {
            //Arrange
            var query = new PartnerUserListQuery(
                (ofxUserGuid: null, partnerAppId: null, partnerUserId: null, beneficiaryId: null)
            ) {ReturnAllResults = true};

            //Act
            var results = _queryService.Execute(query, out var totalCount);

            //Assert
            Assert.Equal(_partnerUsersData.Count(), totalCount);
            Assert.Equal(totalCount, results.Count());
        }
    }
}
