#region Copyright Notice
//Copyright 2002-2016 Software Conceptions, Inc 4103 Cornell Rd. 
//Okemos. MI 49964, U.S.A. All rights reserved.

//Software Conceptions, Inc has intellectual property rights relating to 
//technology embodied in this product. In particular, and without 
//limitation, these intellectual property rights may include one or more 
//of U.S. patents or pending patent applications in the U.S. and/or other countries.

//This product is distributed under licenses restricting its use, copying and
//distribution. No part of this product may be 
//reproduced in any form by any means without prior written authorization 
//of Software Conceptions.

//Software Conceptions is a trademarks of Software Conceptions, Inc
#endregion
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class ObjectEquality
  {
    public static List<string> GetKeys<T>(T x, T y) where T : class
    {
      return (from KeyValuePair<string, object> item in GetComparisonResultKeys(x, y)
              select item.Key).ToList<string>();
    }

    public static IEnumerable<KeyValuePair<string, object>> GetComparisonResultKeys<T>(T x, T y) where T : class
    {
      var d1 = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(x));
      var d2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(y));

      return d1.Except(d2).Union(d2.Except(d1));
    }

    public static bool AreEqual<T>(T x, T y) where T : class
    {
      return GetComparisonResultKeys(x, y).Count() == 0;
    }
		
    public static bool HasFieldChanged<T>(T x, T y, string fieldname) where T: class
    {
      try
      {
        return GetKeys(x, y).Contains(fieldname);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return true;
      }
    }
  }
}
