using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Atlas
{
  public class Result
  {
    public string Type { get; set; }
    public string Id { get; set; }
    public double Score { get; set; }
    public Address Address { get; set; }
    public Position Position { get; set; }
    public ViewPort ViewPort { get; set; }
    public List<EntryPoint> EntryPoints { get; set; }
  }
}
