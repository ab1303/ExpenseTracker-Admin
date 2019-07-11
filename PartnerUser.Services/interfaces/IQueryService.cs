namespace PartnerUser.Services.Interfaces
{
    public interface IQueryService
    {
        TResult Execute<TResult>(IQuery<TResult> query, out int totalCount);
    }
}
