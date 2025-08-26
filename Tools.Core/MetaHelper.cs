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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools
{
  public class MetaHelper
  {
    public static List<dynamic> GetList<T>(bool happy) where T : struct
    {
      var values = Enum.GetValues(typeof(T));

      return (from T item in values.AsQueryable()
              let name = GetName(item, happy)
              orderby name
              select new { ID = Convert.ToInt32(item), Value = name }).ToList<dynamic>();
    }

    public static List<dynamic> GetKeyList<T>(bool happy) where T : struct
    {
      var values = Enum.GetValues(typeof(T));

      return (from T item in values.AsQueryable()
              let name = GetName(item, happy)
              orderby name
              select new { Key = Convert.ToInt32(item), Value = name }).ToList<dynamic>();
    }

    public static List<dynamic> GetKeyListByValue<T>(bool happy) where T : struct
    {
      var values = Enum.GetValues(typeof(T));

      return (from T item in values.AsQueryable()
              let id = Convert.ToInt32(item)
              orderby id
              select new { Key = Convert.ToInt32(item), Value = GetName(item, happy) }).ToList<dynamic>();
    }

    public static string GetName<T>(T item, bool happy)
    {
      string s = Enum.GetName(typeof(T), item);
      if (happy == false) return s;

      return Regex.Replace(s, "^[0-9]", "").Replace("_", " ");
    }

    public static T GetValue<T>(int value)
    {
      return (T)Enum.ToObject(typeof(T), value);
    }

    public static object GetKeyA<T>(int key) where T : new()
    {
      var e = Convert.ChangeType(Enum.Parse(typeof(T), Enum.GetName(typeof(T), key)), typeof(T));
      ((Enum)e).GetAttribute<MetaAttribute>().Dump();
      MetaAttribute a = ((Enum)e).GetAttribute<MetaAttribute>();
      return a.IsDecimal ? (object)a.GetDecimal() : (object)a.GetGuid();
    }

    public static object GetKeyS<T>(string name) where T : struct
    {
      try
      {
        var list = MetaHelper.GetKeyList<T>(true);
        var result = TypeMagic.CastByExample(list, new { Key = 0, Value = "" });

        var o = result.Find(x => string.Compare(x.Value, name, true) == 0);
        return MetaHelper.GetKeyA<T>(o.Key);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return null;
      }
    }

    public static object GetKeyI<T>(string name) where T : struct
    {
      try
      {
        var list = Enum.GetNames(typeof(T));

        var x = list.FirstOrDefault(o => string.Compare(o, name, true) == 0);
        if (x == null) return null;
        return (int)Enum.Parse(typeof(T), name, true);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return null;
      }
    }
   
    public static object GetKey<T>(int value)
    {
      var values = Enum.GetValues(typeof(T));
      foreach (var item in values)
        if ((int)item == value)
          return GetKey((Enum)item);

      return value;
    }

    public static object GetKey(Enum e)
    {
      try
      {
        return e.GetAttribute<MetaAttribute>().ID;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Convert.ToInt32(e);
      }
    }
  }
}
