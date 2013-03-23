using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace Pinger
{
  class Program
  {
    static int Main(string[] args)
    {
      return new Program().PerformPing(args);
    }

    PingerCommandLine cl = new PingerCommandLine();

    int PerformPing(string[] args)
    {
      if (!cl.ParseAndContinue(args)) return -1;
      cl.ShowLogo();

      IPAddress first = GetIPAddress(cl.startingAddress);
      IPAddress second = GetIPAddress(cl.endingAddress);

      if (first == null)
      {
        Console.WriteLine("Could not parse the first IP Address");
        return -1;
      }
      else if (second == null)
      {
        DoPing(first);
      }
      else if (GetAddress(first) < GetAddress(second))
      {
        DoPing(first, second);
      }
      else
      {
        DoPing(second, first);
      }

      return 0;
    }

    long GetAddress(IPAddress addr)
    {
      byte[] addrParts = addr.GetAddressBytes();
      if (addrParts.Length != 4) throw new InvalidCastException("non-IPv4 addresses are not supported");

      return addrParts[0] << 24 | 
             addrParts[1] << 16 | 
             addrParts[2] << 8 | 
             addrParts[3];  
    }

    void DoPing(IPAddress start, IPAddress end)
    {
      byte[] first = start.GetAddressBytes();
      byte[] last = end.GetAddressBytes();

      for (byte a = first[0]; a <= last[0]; ++a)
      {
        for (byte b = first[1]; b <= last[1]; ++b)
        {
          for (byte c = first[2]; c <= last[2]; ++c)
          {
            for (byte d = first[3]; d <= last[3]; ++d)
            {
              DoPing(new IPAddress(new byte[] { a, b, c, d}));
            }
          }
        }
      }

    }

    void DoPing(IPAddress addr)
    {
      Ping ping = new Ping();
      for (int x = 0; x < cl.repeats; ++x)
      {
        Console.Write("Pinging {0}...", addr);
        PingReply reply = ping.Send(addr);
        if (reply.Status == IPStatus.Success)
        {
          Console.WriteLine("Complete ({0}ms)", reply.RoundtripTime);
        }
        else
        {
          Console.WriteLine("Failed - {0}", reply.Status.ToString());
        }
      }
    }

    IPAddress GetIPAddress(string theAddress)
    {
      if (theAddress == string.Empty) return null;

      IPAddress addr = null;
      if (IPAddress.TryParse(theAddress, out addr))
      {
        return addr;
      }

      // Lookup
      IPHostEntry entry = Dns.GetHostEntry(theAddress);
      return entry.AddressList[0];
    }
  }
}
