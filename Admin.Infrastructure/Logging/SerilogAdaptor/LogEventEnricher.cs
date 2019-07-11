using Admin.Infrastructure.Logging.SerilogAdaptor.interfaces;

namespace Admin.Infrastructure.Logging.SerilogAdaptor
{
    public class LogEventEnricher : ILogEventEnricher
    {
        public string Name { get; }
        public object Value { get; }
        public bool DestructureObjects { get; }

        public LogEventEnricher(string name, object value, bool destructureObjects = false)
        {
            Name = name;
            Value = value;
            DestructureObjects = destructureObjects;
        }
    }
}
