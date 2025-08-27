using Tools;
using Tools.Core;
using Xunit.Abstractions;

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
    public void GetFirstSetWithJokerTest()
    {
      Assert.Fail("Not implemented yet");
    }
  }
}