using System.Linq;
using System.Threading;
using Tools;
using Tools.Core;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RummiKub.GamePlay.Test
{
  public class TileTests
  {
    private readonly ITestOutputHelper output;
    private readonly TestOutputWriter writer;

    public TileTests(ITestOutputHelper output)
    {
      this.output = output;
      this.writer = new TestOutputWriter(output);
    }

    [Fact]
    public void GetPoolTest()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      pool.Count.Dump(writer);
      pool.Dump(writer);
      
      pool.Given(pool.Count == 106, writer);
      pool.Expect();
    }

    [Fact]
    public void ContainsSetTest()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.ContainsSet(pool);
      o.Given(o, writer);
      o.Expect();
    }

    [Fact]
    public void ContainsRunTest()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.ContainsRun(pool);
      o.Given(o, writer);
      o.Expect(writer);
    }

    [Fact]
    public void GetFirstSetTest()
    { 
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.GetFirstSet(pool);
      o.Dump(writer);

      o.Given(o.Count > 0, writer);
      o.Expect();
    }

    [Fact]
    public void GetFirstRunTest()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.GetFirstRun(pool);
      o.Dump(writer);

      o.Given(o.Count > 0, writer);
      o.Expect();
    }

    [Fact]
    public void DrawRunTest()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.GetFirstRun(pool);
      o.Dump(writer);

      o.Given(o.Count > 0, writer);
      o.Expect();
      WindowRenderer.DrawRun(o);
    }

    [Fact]
    public void GetCardName()
    {
      TilePool.Init();
      var pool = TilePool.Pool;
      var o = TileSet.GetFirstRun(pool);
      var card = o.First();
      card.Name.Dump(writer);
      card.CardName.Dump(writer);

      card.Given(card.CardName == "AH", writer);
      card.Expect(writer);
    }

    [SkippableFact]
    public void GetFirstSetWithJokerTest()
    {
      TilePool.Init();
      var pool = TilePool.GetStartingHandWithJoker(1);
      var set = pool.GetFirstSetWithJoker();
      set.Dump(writer);
      if (set.Count == 0)
      {
        throw Xunit.Sdk.SkipException.ForSkip("Set is empty");
      }
      set.Given(set.Count > 0, writer);
      set.Expect(writer);
    }
  }
}