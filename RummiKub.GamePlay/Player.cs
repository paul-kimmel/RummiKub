using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace RummiKub.GamePlay
{
  public class Player(string name, List<Tile> hand)
  {

    public List<Tile> Hand { get; set; } = hand;
    public string Name { get; set; } = name;

    public int Count => Hand.Count;
    public string GetHand()
    {
      return string.Join(",", Hand);
      
    }

  }
}
