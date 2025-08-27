namespace RummiKub.GamePlay
{
  public class Tiles : ITiles
  {
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
            if (run.Count >= 3) return run;
            run.Clear();
            run.Add(current);
          }
        }

        if (run.Count >= 3) return run;
      }

      return new List<Tile>();
    }


    public List<Tile> GetTiles()
    {
      return List;
    }
  }
}
