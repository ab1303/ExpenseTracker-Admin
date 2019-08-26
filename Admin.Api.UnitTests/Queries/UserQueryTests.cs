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
    public class UserQueryTests
    {
        private readonly IQueryService _queryService;
        private readonly IEnumerable<Domain.Model.User> _usersData;

        public UserQueryTests()
        {
            _usersData = new MockedDataStore().UsersData();

            var partnerUserRepositoryMock = new Mock<IUserRepository>();
            partnerUserRepositoryMock
                .Setup(_ => _.GetAll()).Returns(_usersData.AsQueryable());

            _queryService = new QueryService(new MockRepositories(partnerUserRepositoryMock.Object));
        }

        [Fact(DisplayName = "UserListQuery filters results using defaults when no query parameters are provided")]
        public void UserListQuery_returns_all_results_using_defaults_when_no_query_parameters_are_provided()
        {
            //Arrange
            var query = new UserListQuery((email: null, firstName: null));

            //Act
            var results = _queryService.Execute(query, out var totalCount);

            //Assert
            Assert.Equal(_usersData.Count(), totalCount);
            Assert.Equal(results.Count(), PagedResourceBase.DefaultPageSize);
        }

        [Fact(DisplayName = "UserListQuery filters results by pageSize given pageSize")]
        public void UserListQuery_filters_results_by_pageSize_given_pageSize()
        {
            //Arrange
            var pageSize = PagedResourceBase.DefaultPageSize + 3;
            var query = new UserListQuery(
                (email: null, firstName: null),
                (pageNumber: 1, size: pageSize)
            );

            //Act
            var results = _queryService.Execute(query, out var _);

            //Assert
            var users = results as Domain.Model.User[] ?? results.ToArray();
            Assert.Equal(pageSize, users.Length);
            Assert.True(users.Length > PagedResourceBase.DefaultPageSize);
        }

        [Fact(DisplayName = "UserListQuery filters results by maxPageSize given pageSize greater than maxPageSize")]
        public void UserListQuery_filters_results_by_maxPageSize_given_pageSize_greater_than_maxPageSize()
        {
            //Arrange
            var pageSize = PagedResourceBase.MaxPageSize + 3;
            var query = new UserListQuery(
                (email: null, firstName: null),
                (pageNumber: 1, size: pageSize)
            );

            //Act
            var results = _queryService.Execute(query, out var _);

            //Assert
            var users = results as Domain.Model.User[] ?? results.ToArray();
            Assert.True(users.Length <= PagedResourceBase.MaxPageSize);
            Assert.True(users.Length < pageSize);
        }
    }
}
