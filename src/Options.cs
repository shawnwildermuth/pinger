using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace Pinger
{
  public class Options
  {
    [Value(0, MetaName = "first", Required = true, HelpText = "Starting IP Address (or DNS Name)")]
    public string FirstAddress { get; set; }

    [Value(1, MetaName = "last",  Required = true, HelpText = "Ending IP Address (or DNS Name)")]
    public string LastAddress { get; set; }

    [Option('r', "repeats", Required = false, HelpText = "Number of times to repeat the pings")]
    public int Repeats { get; set; } = 1;

    [Usage(ApplicationAlias = "pinger")]
    public static IEnumerable<Example> Examples
    {
      get
      {
        return new List<Example>() {
          new Example("Ping machines in your local network", 
            new Options { FirstAddress = "192.168.1.1", LastAddress = "192.168.1.15" })
        };
      }
    }


  }
}
