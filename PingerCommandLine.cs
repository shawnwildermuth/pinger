using System;
using System.Collections.Generic;
using System.Text;

using Genghis;
using CLP = Genghis.CommandLineParser;

namespace Pinger
{
  [CLP.ParserUsage("A Better Ping...IMHO", PreferredPrefix="/", ShowCategories=true)]
  internal class PingerCommandLine : CLP
  {
    [CLP.ValueUsage("starting address", MatchPosition = true, Optional = false, Category="Addresses")]
    public string startingAddress = "";

    [CLP.ValueUsage("ending address", MatchPosition = true, Optional = true, Category = "Addresses")]
    public string endingAddress = "";

    [CLP.ValueUsage("repeat", Name = "r", Optional = true, Category = "Behavior")]
    public int repeats = 1;

    public void ShowLogo()
    {
      Console.WriteLine(GetLogo());
    }
  }
}
