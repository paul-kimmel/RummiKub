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
      var pool = TilePool.Pool;
      pool.Count.Dump(writer);
      pool.Dump(writer);
      
      pool.Given(pool.Count == 106, writer);
      pool.Expect();



    }
  }
}