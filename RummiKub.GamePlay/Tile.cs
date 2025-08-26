using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKub.GamePlay
{
  public class Tile
  {
    public TileValue Value { get; set; }
    public TileColor Color { get; set; }

    public string Name => ToString();

    public bool IsJoker()
    {
      return GetScore() == 30;
    }

    public int GetScore()
    {
      return (int)Value;
    }

    public static Tile GetJoker()
    {
      return new Tile() { Value = TileValue.Joker, Color = TileColor.Red };
    }

    public override string ToString()
    {
      return Value == TileValue.Joker ? $"{Value} {Color}" : $"{(int)Value} {Color}";
    }
  }
}
