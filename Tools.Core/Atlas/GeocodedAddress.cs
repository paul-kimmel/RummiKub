using System;
using System.Collections.Generic;
using System.Linq;

namespace Tools.Atlas
{
  public class GeocodedAddress
  {
    public Summary Summary { get; set; }
    public List<Result> Results { get; set; }

  }
}
