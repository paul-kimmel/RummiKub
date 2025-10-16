using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKub.GamePlay
{

  public class TileSet : Tiles 
  {
    public override int GetScore()
    {
      return GetSetScore();
      
    }
  }
}
