namespace RummiKub.GamePlay
{
  public static class TilePool
  {
    private static readonly int STARTING_HAND_SIZE = 14;

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

    public static Tile GetCard(string name)
    {
      var index = Pool.FindIndex(o => o.CardName == name);
      var o = Pool[index];
      Pool.RemoveAt(index);
      return o;
    }

    public static List<Tile> GetStartingHand()
    {
      var list = new List<Tile>();

      for (var i = 0; i < STARTING_HAND_SIZE; i++)
      {
        list.Add(TilePool.GetRandomTile());
      }

      return list;
    }

    public static List<Tile> GetStartingHandWithJoker(int jokerCount = 0)
    {
      Guard(jokerCount >= 0 && jokerCount <= 2, () => throw new ArgumentOutOfRangeException(nameof(jokerCount)));
      //for testing
      var list = new List<Tile>();

      for (var i = 0; i < jokerCount; i++)
      {
        list.Add(GetCard("Joker"));
      }

      for (var i = 0; i < 14 - jokerCount; i++)
      {
        list.Add(TilePool.GetRandomTile());
      }
      return list;
    }

    static void Guard(bool v, Action action)
    {
      if (!v)
        action();
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
