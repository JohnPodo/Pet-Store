using PetMeUp.Handlers;

namespace PetMeUp.Services
{
    public class CleanerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<CleanerService> _logger;
        private LogHandler handler;
        private PicHandler _Pichandler;
        private Timer _timer = null!; 
        protected readonly string _conString;
        protected readonly string _dbtype;

        public CleanerService(ILogger<CleanerService> logger,IConfiguration config)
        {
            _logger = logger;
            _conString = config.GetConnectionString("ConString");
            _dbtype = config.GetConnectionString("DbType"); 
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
            handler = new LogHandler(_conString, _dbtype);
            _Pichandler = new PicHandler(_conString, _dbtype, handler);
            var result = handler.DeleteAllLogs().Result;
            if (!result) handler.WriteToLog("Something went wrong while cleaning logs", Models.Severity.Error);
            var count = Interlocked.Increment(ref executionCount);
            var path = Path.Combine(Environment.CurrentDirectory, "Images");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    FileInfo fi = new FileInfo(file); 
                    var pic = _Pichandler.GetPic(fi.Name, false).Result;
                    if (pic is null) File.Delete(file);
                }
            }
            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);

            handler.Dispose();
            _Pichandler.Dispose(); 
            handler = null;
            _Pichandler = null; 
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }
    }
}
