using PetMeUp.Handlers;

namespace PetMeUp.Services
{
    public class LogCleanerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<LogCleanerService> _logger;
        private readonly LogHandler handler;
        private Timer _timer = null!;

        public LogCleanerService(ILogger<LogCleanerService> logger,IConfiguration config)
        {
            _logger = logger; 
            handler = new LogHandler(config.GetConnectionString("ConString"), config.GetConnectionString("DbType"));
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(60));

            return Task.CompletedTask;
        }
        private void DoWork(object? state)
        {
            var result = handler.DeleteAllLogs().Result;
            if (!result) handler.WriteToLog("Something went wrong while cleaning logs", Models.Severity.Error);
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
