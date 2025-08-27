using System.Diagnostics;

namespace RummiKub.GamePlay
{
  public static class TileExtensions
  {
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
  }

}
