using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Tools
{
  public static class PictureTools
  {
    public static byte[] GetPictureBytes(this Image o)
    {
      if (o == null)
        throw new ArgumentNullException("Image");

      using (var stream = new MemoryStream())
      {
        var saveFormat = o.RawFormat.Equals(ImageFormat.MemoryBmp) ? ImageFormat.Bmp : o.RawFormat;
        o.Save(stream, saveFormat);
        return stream.ToArray();
      }
    }

    public static Bitmap ToBitmap(this byte[] o)
    {
      using(var stream = new MemoryStream(o))
      {
        return (Bitmap)Bitmap.FromStream(stream);
      }
    }

    public static float GetAspectRatio(this Image o)
    {
      if (o == null)
        throw new ArgumentNullException("Image");
      try
      {
        float result = (float)o.Height / (float)o.Width;
        return result > 1.0f ? 1.0f : result;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return 1.0f;
      }
    }

    public static Image GetThumbnail(this Image o, int width)
    {
      if (o == null)
        throw new ArgumentNullException("Image");

      try
      {
        return o.GetThumbnailImage(width, (int)(width * o.GetAspectRatio()), null, IntPtr.Zero);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return new Bitmap(1, 1);
      }
    }

    public static Image GetThumbnail2(this Image o, float width, float height)
    {
      if (o == null)
        throw new ArgumentNullException("Image");

      var thumb = new Bitmap((int)width, (int)height);
      using (var graphics = Graphics.FromImage(thumb))
      {
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

        var rectangle = new RectangleF(0, 0, width, height);
        graphics.DrawImage(o, rectangle);
        return thumb;
      }
    }

    public static Image ToImage(this string picture)
    {
      if (string.IsNullOrWhiteSpace(picture))
        return null;
      try
      {
        picture = picture.Replace("data:image/png;base64,", "");

        var bytes = Convert.FromBase64String(picture);
        return bytes.ToBitmap();
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return null;
      }
    }

    //BUGFIX: 5014 - Dropped thumbnail conversion
    public static string GetBase64PictureString_BugFix(this Image o, float width = 48f, float height = 48f)
    {
      try
      {
        return $"data:image/png;base64,{Convert.ToBase64String(o.GetPictureBytes())}";
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return "";
      }
    }


    public static string GetBase64PictureString(this Image o, float width = 48f, float height = 48f)
    {
      try
      {
        return $"data:image/png;base64,{Convert.ToBase64String(o.GetThumbnail2(width, height).GetPictureBytes())}";
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return "";
      }
    }
  }
}
