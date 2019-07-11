using PartnerUser.Repositories;

namespace PartnerUser.Services.Interfaces
{
    public interface IQuery<out TResult>
    {
        TResult GetResult(IRepositories repositories, out int totalCount);
    }
}
