using Tools;
using Tools.Core;
using Xunit.Abstractions;

namespace RummiKub.GamePlay.Test
{
  public class PlayerTests
  {

    private readonly ITestOutputHelper output;
    private readonly TestOutputWriter writer;

    public PlayerTests(ITestOutputHelper output)
    {
      this.output = output;
      this.writer = new TestOutputWriter(output);
    }

  
  }
}