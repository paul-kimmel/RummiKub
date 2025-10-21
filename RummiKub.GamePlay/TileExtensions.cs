using FluentValidation.Validators;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

    public static List<List<Tile>> GetAllRunsWithJoker(this List<Tile> tiles)
    {
      if (tiles == null || tiles.Count == 0) return new List<List<Tile>>();

      var clone = tiles.Clone();
      var result = new List<List<Tile>>();

      while (true)
      {
        // this add the joker to each possible but allows it to be reused for each possible run
        var run = Tiles.GetFirstRun(clone);

        if (run.Count() > 0)
        {
          run = run.AddJokers(clone);
          result.Add(run);
          //do not remove joker
          clone.RemoveSetSkipJoker(run);
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
      return Tiles.GetFirstRun(tiles).AddJoker(tiles);
    }

    public static List<Tile> AddJokers(this List<Tile> run, List<Tile> tiles)
    {
      foreach(var joker in tiles.Where(o => o.IsJoker()))
      {
        run = run.AddJoker(tiles);
      }
      return run;
    }

    public static List<Tile> AddJoker(this List<Tile> run, List<Tile> tiles)
    {
      if (run.Count >= 2 && HasJoker(tiles))
      {
        if (IsFullRun(run))
        {
          //Noop
        }
        else if (RunEndsInThirteen(run))
        {
          PrependJoker(tiles, run);
        }
        else
        {
          AppendJoker(tiles, run);
        }
      }

      return run;
    }

    private static void AppendJoker(List<Tile> tiles, List<Tile> run)
    {
      var index = tiles.FindIndex(o => o.IsJoker());
      run.Add(tiles[index].Clone());
    }

    private static void PrependJoker(List<Tile> tiles, List<Tile> run)
    {
      var index = tiles.FindIndex(o => o.IsJoker());
      run.Insert(0, tiles[index].Clone());
    }

    private static bool RunEndsInThirteen(List<Tile> run)
    {
      //BUG: This doesn't work with multi Jokers. If the run ends in 12 and a joker is added then it will still be a joker until scoring. Logic fail.
      return run.GetTilePseudoValues()[^1].Value == TileValue.Thirteen;
    }
    public static List<Tile> GetTilePseudoValues(this List<Tile> tiles)
    {
      try
      {
        int startIndex = Array.FindIndex(tiles.ToArray(), n => (int)n.Value != 30);
        int startValue = (int)tiles[startIndex].Value;
        TileColor startColor = tiles[startIndex].Color;

        //backfill
        for (int i = startIndex - 1; i >= 0; i--)
          tiles[i].Value = (TileValue)tiles[i + 1].Value - 1;

        //forward fill
        for (int i = startIndex + 1; i < tiles.Count; i++)
        {
          if ((int)tiles[i].Value == 30)
          {
            tiles[i].Value = (TileValue)tiles[i - 1].Value + 1;
            tiles[i].Color = startColor;
          }
        }

        return tiles;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return new List<Tile>();
      }
    }


    private static bool IsFullRun(List<Tile> run)
    {
      return run[0].Value == TileValue.One && run[^1].Value == TileValue.Thirteen;
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
        return tiles.Find(o => o.IsJoker()) is not null;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return false;
      }
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

    public static void RemoveSetSkipJoker(this List<Tile> parent, List<Tile> set)
    {
      foreach (var o in set)
      {
        if(o is not null && o.IsJoker()) continue;
        parent.Remove(o);
      }
    }

    public static bool ContainsJoker(this List<Tile> list)
    {
      return list.Any(o => o.IsJoker());
    }

    public static int GetJokerCount(this List<Tile> list)
    {
      return list.Count(o => o.IsJoker());
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
