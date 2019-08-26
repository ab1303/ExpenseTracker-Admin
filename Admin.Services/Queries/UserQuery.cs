using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Admin.Common.Enums;
using Admin.Common.FunctionalExtensions;
using Admin.Common.Models;
using Admin.Repositories;
using Admin.Services.interfaces;

namespace Admin.Services.Queries
{

    public class UserListQuery : IQuery<IEnumerable<Admin.Domain.Model.User>>
    {
        private readonly (string email, string firstName) _filters;

        public bool ReturnAllResults { get; set; } = false;
        public (int pageNumber, int size) Pagination { get; }
        public (SortOrder order, string column) Sorting { get; }

        public UserListQuery(
            (string email, string firstName) filters,
            (int pageNumber, int size)? pagination = null,
            (SortOrder order, string column)? sorting = null
            )
        {
            _filters = filters;
            Pagination = pagination == null
                ? (pageNumber: PagedResourceBase.DefaultPageNumber, size: PagedResourceBase.DefaultPageSize)
                : (pageNumber: pagination.Value.pageNumber < 1
                        ? PagedResourceBase.DefaultPageNumber
                        : pagination.Value.pageNumber,
                    size: pagination.Value.size > PagedResourceBase.MaxPageSize
                        ? PagedResourceBase.MaxPageSize
                        : pagination.Value.size)
                ;
            Sorting = sorting ?? (order: SortOrder.Ascending, column: "CreatedDate");
        }

        public IEnumerable<Admin.Domain.Model.User> GetResult(IRepositories repositories, out int totalCount)
        {
            var orderBy = $"{Sorting.column} {(Sorting.order == SortOrder.Ascending ? "ASC" : "DESC")}";

            var partnerUserQuery = repositories.UserRepository.GetAll()
                    .FilterRepositoryBy(() => ! string.IsNullOrWhiteSpace(_filters.email) , pu => pu.Email == _filters.email)
                ;

            totalCount = partnerUserQuery.Count();

            var partnerUserList = ReturnAllResults
                ? partnerUserQuery.OrderBy(orderBy).ToArray()
                : partnerUserQuery.OrderBy(orderBy)
                .Skip(Pagination.size * (Pagination.pageNumber - 1))
                .Take(Pagination.size)
                .ToArray();

            return partnerUserList;
        }
    }
}
