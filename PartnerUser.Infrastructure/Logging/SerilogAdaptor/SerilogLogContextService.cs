using System;
using System.Linq;
using PartnerUser.Infrastructure.Logging.SerilogAdaptor.interfaces;
using Serilog.Context;
using Serilog.Core.Enrichers;

namespace PartnerUser.Infrastructure.Logging.SerilogAdaptor
{
    public class SerilogLogContextService : ILogContextService
    {
        public IDisposable PushProperty(string name, object value, bool destructureObjects = false)
        {
            return LogContext.PushProperty(name, value, destructureObjects);
        }

        public IDisposable PushProperties(params ILogEventEnricher[] properties)
        {
            var serilogProperties = (properties ?? new ILogEventEnricher[0])
                .Select(p => (Serilog.Core.ILogEventEnricher)new PropertyEnricher(p.Name, p.Value, p.DestructureObjects))
                .ToArray();
            return LogContext.Push(serilogProperties);
        }
    }
}
