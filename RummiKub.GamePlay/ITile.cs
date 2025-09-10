namespace RummiKub.GamePlay
{
  public interface ITile
  {
    static abstract Tile Empty { get; }
    TileColor Color { get; set; }
    string Name { get; }

    string CardName { get; }

    TileValue Value { get; set; }

    static abstract Tile GetJoker();
    bool Equals(object obj);
    int GetHashCode();
    int GetScore();
    bool IsJoker();
    string ToString();
  }
}