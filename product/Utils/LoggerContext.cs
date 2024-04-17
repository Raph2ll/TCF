using System;
using System.Diagnostics;
using Serilog;

namespace product.Utils
{
    public class LoggerContext : IDisposable
    {
        private readonly string _name;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Serilog.ILogger _logger;

        public LoggerContext(string name, Serilog.ILogger logger)
        {
            _name = name;
            _logger = logger;
            _stopwatch.Start();

            try
            {
                _logger?.Information($"Starting {_name}");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, $"Error starting {_name}");
            }
        }

        public void Dispose()
        {
            _stopwatch.Stop();

            try
            {
                _logger?.Information($"{_name} took {_stopwatch.Elapsed.TotalMilliseconds:0.###}ms");
            }
            catch (Exception ex)
            {
                _logger?.Error(ex, $"Error logging {_name} duration");
            }
        }
    }

    public class ContextFactory
    {
        private readonly Serilog.ILogger _logger;

        public ContextFactory(Serilog.ILogger logger)
        {
            _logger = logger;
        }

        public LoggerContext Create(string name)
        {
            return new LoggerContext(name, _logger);
        }
    }
}
