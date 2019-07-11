namespace Admin.Services.interfaces
{
    public interface IQueryService
    {
        TResult Execute<TResult>(IQuery<TResult> query, out int totalCount);
    }
}
