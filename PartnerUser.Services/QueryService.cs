using PartnerUser.Repositories;
using PartnerUser.Services.Interfaces;

namespace PartnerUser.Services
{
    public class QueryService: IQueryService
    {
        private readonly IRepositories _repositories;

        public QueryService(IRepositories repositories)
        {
            _repositories = repositories;
        }
    
        public TResult Execute<TResult>(IQuery<TResult> query, out int totalCount) => query.GetResult(_repositories, out totalCount);
    }
}
