using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using PartnerUser.Common.Enums;
using PartnerUser.Common.FunctionalExtensions;
using PartnerUser.Common.Models;
using PartnerUser.Repositories;
using PartnerUser.Services.Interfaces;

namespace PartnerUser.Services.Queries
{

    public class PartnerUserListQuery : IQuery<IEnumerable<Domain.Model.PartnerUser>>
    {
        private readonly (Guid? ofxUserGuid, Guid? partnerAppId, Guid? partnerUserId, Guid? beneficiaryId) _filters;

        public bool ReturnAllResults { get; set; } = false;
        public (int pageNumber, int size) Pagination { get; }
        public (SortOrder order, string column) Sorting { get; }

        public PartnerUserListQuery(
            (Guid? ofxUserGuid, Guid? partnerAppId, Guid? partnerUserId, Guid? beneficiaryId) filters,
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

        public IEnumerable<Domain.Model.PartnerUser> GetResult(IRepositories repositories, out int totalCount)
        {
            var orderBy = $"{Sorting.column} {(Sorting.order == SortOrder.Ascending ? "ASC" : "DESC")}";

            var partnerUserQuery = repositories.PartnerUserRepository.GetAll()
                    .FilterRepositoryBy(() => _filters.ofxUserGuid.HasValue, pu => pu.OfxUserGuid == _filters.ofxUserGuid.Value)
                    .FilterRepositoryBy(() => _filters.partnerAppId.HasValue, pu => pu.PartnerAppId == _filters.partnerAppId.Value)
                    .FilterRepositoryBy(() => _filters.partnerUserId.HasValue, pu => pu.PartnerUserId == _filters.partnerUserId.Value)
                    .FilterRepositoryBy(() => _filters.beneficiaryId.HasValue, pu => pu.BeneficiaryId == _filters.beneficiaryId.Value)
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
