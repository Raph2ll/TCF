using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Serilog;


namespace client.Utils
{
    public class LoggerContext : IDisposable
    {
        private readonly string? _name;
        private readonly Stopwatch _sw = new Stopwatch();

        private readonly Serilog.ILogger? _logger = null;
        public LoggerContext(string name, Serilog.ILogger looger)
        {
            _sw.Start();
            try
            {
                _name = name;
                _logger = looger;
                _logger?.Information($"Starting {_name}");
            }
            catch (Exception ex)
            {
                OnError(ex.Message);
            }

        }
        private void OnError(string message)
        {
            _sw.Stop();
            _logger?.Error(message);
        }

        public void Dispose()
        {
            _sw.Stop();
            _logger?.Information($"{_name} took {(_sw.Elapsed.TotalMilliseconds):0.###}ms");
        }
    }
    public class ContextFactory
    {
        private readonly Serilog.ILogger? _logger;

        public ContextFactory(Serilog.ILogger? logger)
        {
            _logger = logger;
        }

        public LoggerContext Create(string name)
        {
            return new LoggerContext(name, _logger);
        }
    }
}