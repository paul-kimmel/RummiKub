using Tools;
using Tools.Core;
using Xunit.Abstractions;

namespace RummiKub.GamePlay.Test
{
  public class GameTests
  {
    private readonly ITestOutputHelper output;
    private readonly TestOutputWriter writer;

    public GameTests(ITestOutputHelper output)
    {
      this.output = output;
      this.writer = new TestOutputWriter(output);
    }

    [Fact]
    public void PlayerInitTest()
    {
      var o = new Game([ "Paul", "Carolyn", "Cynthy" ]);
      o.Dump(writer);

      var paul = o.Players[0];
      paul.Dump(writer);
      writer.WriteLine(paul.GetHand());

      paul.Given(paul.Count == 14);
      paul.Expect();

    }
  }
}