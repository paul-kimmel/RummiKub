namespace RummiKub.GamePlay
{
  public class Game
  {
    public const int MinPlayers = 2;
    public const int MaxPlayers = 4;

#pragma warning disable CS8618
    public Game(string[] playerNames)
    {
      TilePool.Init();
      Init(playerNames);
    }
#pragma warning restore CS8618

    public void Init(string[] playerNames)
    {
      Guard(playerNames);

      Players = new Players();
      foreach(var name in playerNames)
      {
        Players.Add(new Player(name, TilePool.GetStartingHand()));
      }

    }

    private void Guard(string[] playerNames)
    {
      if (playerNames.Length < 2 || playerNames.Length > 4)
        throw new ArgumentException("Pick 2 to 4 players");

    }

    public Players Players { get; set; }
    public List<Tile> Pool =>  TilePool.Pool;

  }
}
