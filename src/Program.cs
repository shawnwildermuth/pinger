using System;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pinger
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
              .WithParsedAsync(async options =>
              {
                  Console.WriteLine($"Pinger");
                  Console.WriteLine("Copyright(C) 2020 Wilder Minds LLC");
                  Console.WriteLine();

                  await RunAsync(options);
              });
        }

        private static async Task RunAsync(Options options)
        {

            return Host.CreateDefaultBuilder()
              .ConfigureServices((b, c) =>
              {
                c.AddSingleton(options);
                c.AddHostedService<PingerService>();
              })
              .ConfigureLogging(bldr =>
              {
                  bldr.ClearProviders();
                  bldr.AddConsole()
                   .SetMinimumLevel(LogLevel.Error);
              })
              .RunConsoleAsync().Wait();
        }
    }
}