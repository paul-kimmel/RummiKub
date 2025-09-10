using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tools;

namespace RummiKub.GamePlay.Test
{
  public class WindowRenderer
  {
    [SupportedOSPlatform("windows")]
    public static void DrawImage(string base64String)
    {
      DrawImage(base64String, 10, 10);
    }


    [SupportedOSPlatform("windows")]
    public static void DrawImage(string base64String, int x, int y)
    {
      using var form = new Form
      {
        Width = 300,
        Height = 200,
        Text = "Test Graphics Form"
      };
      form.Show();

      using var g = form.CreateGraphics();
      g.DrawImage(PictureTools.ToImage(base64String), new Point(x, y));


      System.Threading.Thread.Sleep(2000);
    }

    [SupportedOSPlatform("windows")]
    public static void DrawRun(List<Tile> tiles)
    {
      using var form = new Form
      {
        Width = 600,
        Height = 200,
        Text = "Test Graphics Form"
      };
      form.Show();

      using var g = form.CreateGraphics();
      int i = 0;
      foreach(var tile in tiles)
      { 
        var base64String = CardFactory.GetCard(tile.CardName);
        g.DrawImage(PictureTools.ToImage(base64String), new Point(10 + (i++ * 22), 10));
      }
      
      System.Threading.Thread.Sleep(2000);
    }


  }
}
