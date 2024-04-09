using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using Serilog;
using client.Utils;

namespace client.Db
{
    public class DbContext : IDisposable
    {
        private readonly MySqlConnection _connection;
        private readonly string _name;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Serilog.ILogger _logger;
        private readonly LoggerContext _loggerContext;

        public DbContext(string connectionString, string name, Serilog.ILogger logger)
        {
            _connection = new MySqlConnection(connectionString);

            _name = name;
            _logger = logger;

            _loggerContext = new LoggerContext($"Connecting to database: {_name}", _logger);
            _stopwatch.Start();

            _connection.Open();
        }

        public MySqlConnection Connection => _connection;

        public void Dispose()
        {
            _stopwatch.Stop();
            _connection.Close();

            using (_loggerContext)
            {
                _logger.Information($"Disconnected from database: {_name}. Elapsed time: {_stopwatch.Elapsed.TotalMilliseconds:0.###}ms");
            }
        }
    }
}
