using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tools;
using Cards = Resources.Properties.Resources;

namespace RummiKub.GamePlay
{
  public class CardFactory
  {
    public static readonly Regex Digits = new Regex("[0-9]", RegexOptions.Compiled);
    public static string GetCard(string name)
    {
      try
      {
        if (Digits.IsMatch(name[0].ToString()))
        {
          name = "_" + name;
        }
        var property = typeof(Cards).GetProperty(name);
        var result = (byte[])property.TryGetValue(null);
        return $"data:image/png;base64,{Convert.ToBase64String(result)}";
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return string.Empty;
      }
    }

    public static string GetCardName(ITile tile)
    {
      if(tile.IsJoker())
      {
        return "Joker";
      } 

      return $"{GetCardFace(tile)}{GetCardSuit(tile)}";
    }

    private static string GetCardSuit(ITile tile)
    {
      switch (tile.Color)
      {
        case TileColor.Red:
          return "H";
        case TileColor.Black:
          return "S";
        case TileColor.Cyan:
          return "C";
        case TileColor.Orange:
          return "D";
        default:
          throw new NotImplementedException();
      };
    }

    private static string GetCardFace(ITile tile)
    {
      switch (tile.Value)
      {
        case TileValue.One: return "A";
        case TileValue.Two: return "2";
        case TileValue.Three: return "3";
        case TileValue.Four: return "4";
        case TileValue.Five: return "5";
        case TileValue.Six: return "6";
        case TileValue.Seven: return "7";
        case TileValue.Eight: return "8";
        case TileValue.Nine: return "9";
        case TileValue.Ten: return "10";
        case TileValue.Eleven: return "J";
        case TileValue.Twelve: return "Q";
        case TileValue.Thirteen: return "K";
        default:
          throw new NotImplementedException();
      }
    }
  }


}
