using System;
using Microsoft.Extensions.Logging;

namespace scraper
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel LogLevel { get; }
        
        public ConsoleLogger(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                Console.WriteLine("[{0}] {1}", logLevel, formatter(state, exception));
            }
        }
    }
}