using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RummiKub.GamePlay
{
  public class Tile
  {
    public virtual TileValue Value { get; set; }
    public virtual TileColor Color { get; set; }

    public string Name => ToString();

    public virtual bool IsJoker()
    { 
      //TODO: We need acting value for jokers
      return GetScore() == 30;
    }

    public virtual int GetScore()
    {
      return (int)Value;
    }

    public static Tile GetJoker()
    {
      return new Joker() { Value = TileValue.Joker, Color = TileColor.Red };
    }

    public override string ToString()
    {
      return Value == TileValue.Joker ? $"{Value} {Color}" : $"{(int)Value} {Color}";
    }

    public static bool operator ==(Tile lhs, Tile rhs)
    {
      if (ReferenceEquals(lhs, rhs)) return true;
      if (lhs is null || rhs is null) return false;
      return lhs.Color == rhs.Color && lhs.Value == rhs.Value;
    }

    public static bool operator !=(Tile left, Tile right) => !(left == right);

    public override bool Equals(object obj) => obj is Tile t && this == t;

    public override int GetHashCode() => HashCode.Combine(Color, Value);

    private static readonly Tile _empty = new Tile();
    public static Tile Empty { get { return _empty; } }
  }

}
