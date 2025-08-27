namespace RummiKub.GamePlay
{
  public class Joker : Tile
  {
    public override TileValue Value { get; set; } = TileValue.Joker;
    public override TileColor Color { get; set; } = TileColor.Red;

    public override bool IsJoker() => true;
    
    public void Reset()
    {
      Value = TileValue.Joker;
      Color = TileColor.Red;
    }
  }
}
