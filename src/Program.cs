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
    static void Main(string[] args)
    {
      Parser.Default.ParseArguments<Options>(args)
        .WithParsed(options =>
        {
          Console.WriteLine($"Pinger");
          Console.WriteLine("Copyright(C) 2020 Wilder Minds LLC");
          Console.WriteLine();

          var host = Host.CreateDefaultBuilder()
            .ConfigureServices((b, c) =>
            {
              c.AddSingleton(options);
            })
            .ConfigureLogging(bldr =>
            {
              bldr.ClearProviders();
              bldr.AddConsole()
                .SetMinimumLevel(LogLevel.Error);
            })
            .Build();

          var svc = ActivatorUtilities.CreateInstance<PingerService>(host.Services);
          svc.Run();

        });
    }

  }
}