using System.Collections.Generic;
using System.Diagnostics;

namespace RummiKub.GamePlay
{
  public static class TileExtensions
  {
    public static List<List<Tile>> GetAllSets(this List<Tile> tiles)
    {
      if (tiles == null || tiles.Count == 0) return new List<List<Tile>>();

      var clone = tiles.Clone();
      var result = new List<List<Tile>>();

      while (true)
      {
        var set = Tiles.GetFirstSet(clone);

        if (set.Count() > 0)
        {
          result.Add(set);
          clone.RemoveSet(set);
        }
        else
        {
          break;
        }
      }


      return result;
    }

    public static List<List<Tile>> GetAllRuns(this List<Tile> tiles)
    {
      if (tiles == null || tiles.Count == 0) return new List<List<Tile>>();

      var clone = tiles.Clone();
      var result = new List<List<Tile>>();

      while (true)
      {
        var set = Tiles.GetFirstRun(clone);

        if (set.Count() > 0)
        {
          result.Add(set);
          clone.RemoveSet(set);
        }
        else
        {
          break;
        }
      }


      return result;
    }

    public static List<Tile> GetFirstRunWithJoker(this List<Tile> tiles)
    {
      //Debugger.Break();
      var run = Tiles.GetFirstRun(tiles);
      if (run.Count >= 3 && HasJoker(tiles))
      {
        /* if the lowest is one (1) and the highest is thirteen (13) we can't add the joker
         */
        var index = tiles.FindIndex(o => o.IsJoker());
        if (run[0].Value == TileValue.One && run[run.Count - 1].Value == TileValue.Thirteen)
        {
          //do nothing
        }
        else if (run[^1].Value == TileValue.Thirteen)
        {
          //add at the the beginning
          run.Insert(0, tiles[index]);
        }
        else if(run[0].Value == TileValue.One)
        {
          //add at the end
          run.Add(tiles[index]);
        }
      }

      return run;
    }

    public static List<Tile> GetFirstSetWithJoker(this List<Tile> tiles)
    {
      //Debugger.Break();
      var set = Tiles.GetFirstSet(tiles);
      if (set.Count == 3 && HasJoker(tiles))
      {
        var index = tiles.FindIndex(o => o.IsJoker());
        set.Add(tiles[index]);
      }

      return set;
    }

    public static bool HasJoker(this List<Tile> tiles)
    {
      try
      {
        return tiles.Find(o => o.IsJoker()) != null;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return false;
      }
    }

    public static Tile Clone(this Tile source)
    {
      return new Tile()
      {
        Color = source.Color,
        Value = source.Value
      };
    }

    public static List<Tile> Clone(this List<Tile> source)
    {
      var target = new List<Tile>();
      foreach (var o in source)
      {
        target.Add(o.Clone());
      }

      return target;
    }

    public static void RemoveSet(this List<Tile> parent, List<Tile> set)
    {
      foreach (var o in set)
      {
        parent.Remove(o);
      }
    }

    public static bool ContainsJoker(this List<Tile> list)
    {
      return list.Any(o => o.IsJoker());
    }

    public static Tile RemoveJoker(this List<Tile> list)
    {
      try
      {
        var target = list.Find(o => o.IsJoker());
        var index = list.IndexOf(target);
        var o = list[index];
        list.RemoveAt(index);
        return o;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Tile.Empty;
      }
    }

    public static int GetScore(this List<Tile> tiles)
    {
      try
      {
        return new Tiles() { List = tiles }.GetScore();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 0;
      }
    }

    public static List<Tile> GetFirstSet(this List<Tile> tiles)
    {
      return GetFirstSet(tiles, 30);
    }

    public static List<Tile> GetFirstSet(this List<Tile> tiles, int minimumScore = 30)
    {
      var result = tiles.GetAllSets();

      try
      {
        foreach (var set in result)
        {
          if (set.GetScore() >= minimumScore) return set;
        }

        return new List<Tile>();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return new List<Tile>();
      }
    }


  }

}
