namespace PartnerUser.Infrastructure.Logging.SerilogAdaptor.interfaces
{
    public interface ILogEventEnricher
    {
        string Name { get; }
        object Value { get; }
        bool DestructureObjects { get; }
    }
}
