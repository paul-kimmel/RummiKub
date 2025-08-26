using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class ListFactory
  {
    public static List<T> Create<T>(Func<T> factory, int length = 100) where T : new()
    {
      var list = new List<T>();
      for (int i = 0; i < length; i++)
        list.Add(factory());

      return list;
    }
  }
}
