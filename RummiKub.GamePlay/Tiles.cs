using System.Diagnostics;
using Tools;

namespace RummiKub.GamePlay
{
  public class Tiles : ITiles
  {
    //STOPPED: We need to handle jokers for sets and runs and
    //apply a joker to every possible set and run and then figure out the best, most advantageous placement of the joker and then get rid of the joker where it isn't the best use
    public List<Tile> List { get; set; } = new List<Tile>();

    public bool ContainsRun()
    {
      return ContainsRun(List);
    }

    public bool ContainsSet()
    {
      return ContainsSet(List);
    }

    public static List<Tile> GetFirstSet(List<Tile> tiles)
    { 
      foreach (Tile tile in tiles)
      {
        var result = tiles.Where(o => o.Value == tile.Value).DistinctBy(o => o.Color);

        if(result != null && (result.Count() == 3 || result.Count() == 4))
        {
          return result.ToList();
        }
      }

      return new List<Tile>();
    }

    public static bool ContainsSet(List<Tile> tiles)
    {
      return GetFirstSet(tiles).Count > 0;
    }

    public static bool ContainsRun(List<Tile> tiles)
    {
      return GetFirstRun(tiles).Count > 0;
    }

    public static List<Tile> GetFirstRun(List<Tile> tiles)
    {
      return GetFirstRun(tiles, 3);
    }

    public static List<Tile> GetHighest(List<List<Tile>> series)
    {
      if (series == null || series.Count == 0) return new List<Tile>();
      return series.OrderByDescending(o => o.GetScore()).First();
    }

    public static List<Tile> GetHighestSet(List<Tile> tiles)
    {
      return GetHighest(tiles.GetAllSets());
    }

    public static List<Tile> GetHighestRun(List<Tile> tiles)
    {
      return GetHighest(tiles.GetAllRuns());
    }

    public static List<Tile> GetHighestRunWithJoker(List<Tile> tiles)
    {
      return GetHighest(tiles.GetAllRunsWithJoker());
    }

    public static List<Tile> GetFirstRun(List<Tile> tiles, int runLength = 3)
    {
      if (tiles == null || tiles.Count == 0) return new List<Tile>();

      foreach (var color in Enum.GetValues(typeof(TileColor)))
      {
        var ordered = tiles.Where(o => o.Color == (TileColor)color).OrderBy(o => o.Value).ToList();

        if (ordered.Count == 0) continue;

        var run = new List<Tile> { ordered[0] };

        for (int i = 1; i < ordered.Count; i++)
        {
          var previous = ordered[i - 1];
          var current = ordered[i];

          if (current == previous)
          {
            continue;
          }

          if (current.Value == previous.Value + 1)
          {
            run.Add(current);
          }
          else
          {
            if (run.Count >= runLength) return run;
            run.Clear();
            run.Add(current);
          }
        }

        if (run.Count >= runLength) return run;
      }

      return new List<Tile>();
    }


    public List<Tile> GetTiles()
    {
      return List;
    }

    public virtual int GetScore()
    {
      try
      {
        return ContainsSet() ? GetSetScore() : GetRunScore();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 0;
      }
    }

    
    protected virtual int GetRunScore()
    {
      try
      {
        int startIndex = Array.FindIndex(List.ToArray(), n => (int)n.Value != 30);
        int startValue = (int)List[startIndex].Value;
        TileColor startColor = List[startIndex].Color;

        //backfill
        for (int i = startIndex - 1; i >= 0; i--)
          List[i].Value = (TileValue)List[i + 1].Value - 1;

        //forward fill
        for (int i = startIndex + 1; i < List.Count; i++)
        {
          if ((int)List[i].Value == 30)
          {
            List[i].Value = (TileValue)List[i - 1].Value + 1;
            List[i].Color = startColor;
          }
            
        }

        return List.Sum(x => (int)x.Value);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 0;
      }
    }


    protected virtual int GetSetScore()
    {
      try
      {
        var o = this.List.Where(x => x.IsJoker() == false).First().Value;
        return (int)o * this.List.Count;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 0;
      }
    }
  }
}
