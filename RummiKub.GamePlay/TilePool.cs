namespace RummiKub.GamePlay
{
  public static class TilePool
  {
    static TilePool()
    {
      Pool = GetTiles();
      Shuffle(Pool);
    }

    public static void Init()
    {
      Pool = GetTiles();
      Shuffle(Pool);
    }
            
    public static List<Tile> Pool { get; set; }
    

    public static Tile GetRandomTile()
    {
      var random = new Random(DateTime.Now.Millisecond);
      var index = random.Next(0, Pool.Count);
      var o = Pool[index];
      Pool.RemoveAt(index);
      return o;
    }

    public static List<Tile> GetStartingHand()
    {
      var list = new List<Tile>();

      for(var i = 0; i< 14; i++)
      {
        list.Add(GetRandomTile());
      }

      return list;
    }
    
    public static List<Tile> GetTiles()
    {
      var list = new List<Tile>();
      foreach (TileColor tileColor in Enum.GetValues(typeof(TileColor)))
      {
        for(TileValue tileValue = TileValue.One; tileValue <= TileValue.Thirteen; tileValue++)
        {
          var tile = new Tile() { Value = tileValue, Color = tileColor };
          list.Add(tile);
          list.Add(tile);
        }
      }

      list.Add(Tile.GetJoker());
      list.Add(Tile.GetJoker());


      return list;
    }

    public static void Shuffle<T>(IList<T> list, Random? random = null)
    {
      random ??= Random.Shared; 
      for (int i = list.Count - 1; i > 0; i--)
      {
        int j = random.Next(i + 1);
        (list[i], list[j]) = (list[j], list[i]);
      }
    }
  }

}
