using System.Drawing;
using Tools;
using Tools.Core;
using Xunit.Abstractions;
using Cards = Resources.Properties.Resources;


namespace RummiKub.GamePlay.Test
{

  public class ResourceTests
  {

    private readonly ITestOutputHelper output;
    private readonly TestOutputWriter writer;

    public ResourceTests(ITestOutputHelper output)
    {
      this.output = output;
      this.writer = new TestOutputWriter(output);
    }

        
    [Fact]
    
    public void GetAceOfSpadesTest()
    {
      var o = Cards.AS.ToBitmap();
      o.Dump(writer);
    }

    [Fact]
    public void CardFactoryTest()
    {
      var o = CardFactory.GetCard("10C");
      o.Dump(writer);
      o.Given(!string.IsNullOrEmpty(o), writer);
      o.Expect();
    }
  }
}
