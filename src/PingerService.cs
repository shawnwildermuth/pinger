using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetTools;

namespace Pinger
{
  internal class PingerService : IHostedService
  {
    private readonly ILogger<PingerService> _logger;
    private readonly Options _options;
    private readonly Ping _ping;

    public PingerService(ILogger<PingerService> logger, Options options)
    {
      _logger = logger;
      _options = options;
      _ping = new Ping();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      try
      {
        DoPingRange(cancellationToken);
      }
      catch (Exception ex)
      {
        Console.WriteLine();
        _logger.LogError($"Error while pinging: {ex}");
      }
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    void DoPingRange(CancellationToken cancellationToken)
    {
      var first = GetIPAddress(_options.FirstAddress);
      var last = string.IsNullOrWhiteSpace(_options.LastAddress) ? first : GetIPAddress(_options.LastAddress);

      var range = new IPAddressRange(first, last);

      foreach (var addr in range)
      {
        if (cancellationToken.IsCancellationRequested)
        {
          return;
        }
        DoPing(addr);
      }

    }

    /// <summary>
    /// Perform the ping. Considered async, but we want the results ordered.
    /// </summary>
    /// <param name="addr">The Address to Ping</param>
    void DoPing(IPAddress addr)
    {
      string name = _options.Lookup ? FormatAddress(addr) : $"{addr}";

      for (int repeat = 1; repeat <= _options.Repeats; ++repeat)
      {
        Console.Write($"Pinging {name} ({repeat})...");
        PingReply reply = _ping.Send(addr);
        if (reply.Status == IPStatus.Success)
        {
          Console.WriteLine($"Complete ({reply.RoundtripTime}ms)");
        }
        else
        {
          Console.WriteLine($"Failed - {reply.Status}");
        }
      }
    }

    private string FormatAddress(IPAddress addr)
    {
      var result = $"{addr}";
      try // Hacky but no try...
      {
        var entry = Dns.GetHostEntry(addr);
        result = string.IsNullOrWhiteSpace(entry.HostName) ? $"{addr}" : $"{entry.HostName}({addr})";
      }
      catch // NOOP
      {
      }
      return result;
    }

    IPAddress GetIPAddress(string theAddress)
    {
      if (theAddress == string.Empty) return null;

      if (IPAddress.TryParse(theAddress, out var addr))
      {
        return addr;
      }

      // Lookup
      IPHostEntry entry = Dns.GetHostEntry(theAddress);
      return entry.AddressList[0];
    }

  }
}