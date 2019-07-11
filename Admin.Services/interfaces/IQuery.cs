using Admin.Repositories;

namespace Admin.Services.interfaces
{
    public interface IQuery<out TResult>
    {
        TResult GetResult(IRepositories repositories, out int totalCount);
    }
}
