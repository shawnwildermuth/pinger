using System;
using CommandLine;
using CommandLine.Text;
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

          Host.CreateDefaultBuilder()
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
            .RunConsoleAsync();
        });
      

    }

  }
}
