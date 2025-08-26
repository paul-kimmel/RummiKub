using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class KeyGenerator
  {
    private static Random rand = new Random(DateTime.Now.Millisecond);

    private static char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();
    public static string GenerateKey(int length = 7)
    {
      string result = "";

      for (int j = 0; j < length; j++)
      {
        result += characters[rand.Next(0, characters.Length)].ToString();
      }

      return result;
    }
  }
}
