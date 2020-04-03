using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string path = @"c:\temp\worker_service.txt";

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        private async Task Log(string text)
        {
            await File.AppendAllTextAsync(
                path, DateTime.Now.ToLongTimeString() + ": " + text);
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        { 
            //await Log("Starter servicen");
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Log("Stopper servicen");
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Log("Servicen jobber...");
                await Task.Delay(5000, stoppingToken);
            }
            await Log("Servicen avslutter jobbing");
        }

        public override void Dispose()
        {
            Task.Run(()=>Log("Servicen rydder opp"));
        }
    }
}
