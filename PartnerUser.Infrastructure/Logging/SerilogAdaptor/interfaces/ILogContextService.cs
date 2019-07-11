using System;

namespace PartnerUser.Infrastructure.Logging.SerilogAdaptor.interfaces
{
    public interface ILogContextService
    {
        IDisposable PushProperty(string name, object value, bool destructureObjects = false);
        IDisposable PushProperties(params ILogEventEnricher[] properties);
    }
}
