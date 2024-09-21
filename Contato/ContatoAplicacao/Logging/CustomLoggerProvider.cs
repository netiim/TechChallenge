using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace TemplateWebApiNet8.Logging
{
    public class CustomLoggerProvider : ILoggerProvider
    {
        private readonly CustomLoggerProviderConfiguration loggerConfig;
        private readonly ConcurrentDictionary<string,CustomLogger> loggers = new ConcurrentDictionary<string, CustomLogger> ();

        public CustomLoggerProvider(CustomLoggerProviderConfiguration _loggerConfig)
        {
            loggerConfig = _loggerConfig;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return loggers.GetOrAdd(categoryName, name => new CustomLogger(name, loggerConfig));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
