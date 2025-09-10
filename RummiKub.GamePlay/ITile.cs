namespace RummiKub.GamePlay
{
  public interface ITile : ICardTile
  {
    TileColor Color { get; set; }
    string Name { get; }

    

    TileValue Value { get; set; }

    static abstract Tile GetJoker();
    bool Equals(object obj);
    int GetHashCode();
    int GetScore();
    bool IsJoker();
    string ToString();
  }
}