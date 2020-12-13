using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks; 

public class Background : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<Background> _logger;
    private Timer _timer;
 

    public Background(ILogger<Background> logger)
    {
        _logger = logger;
    }


    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        //QMessageHandler.init().Wait();

        _timer = new Timer(DoWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(10000));


        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        
        var count = Interlocked.Increment(ref executionCount);
         
        QMessageHandler.ReceiveMessagesAsync().Wait();
         
        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
         
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}