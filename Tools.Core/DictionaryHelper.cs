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
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Tools
{
  public class DictionaryHelper
  {
    public static Dictionary<K, V> Clone<K, V>(Dictionary<K, V> original)
    {
      Dictionary<K, V> o = new Dictionary<K, V>(original.Count, original.Comparer);
      foreach (KeyValuePair<K, V> entry in original)
      {
        o.Add(entry.Key, entry.Value);
      }
      return o;
    }

    private static bool IsDictionary<K, V>(Dictionary<K, V> left, Dictionary<K, V> right, K key)
    {
      return left[key] is IDictionary && right[key] is IDictionary;
    }

    public static Dictionary<K, V> Merge<K, V>(Dictionary<K, V> left, Dictionary<K, V> right)
    {
      if (CheckStructuralEquality(left, right) == false)
        throw new Exception("object don't match");

      Dictionary<K, V> result = new Dictionary<K, V>();

      foreach (var key in left.Keys)
      {
        if (IsDictionary<K, V>(left, right, key))
          result.Add(key, left[key]);
        else if (DictionaryEntryTest(left, right, key) == false)
          result.Add(key, left[key]);
        else
          result.Add(key, left[key]);
      }

      return result;
    }


    private static bool CheckSequence<K, V>(Dictionary<K, V> left, Dictionary<K, V> right)
    {
      return left.Keys.SequenceEqual(right.Keys);
    }


    private static bool CheckCount<K, V>(Dictionary<K, V> left, Dictionary<K, V> right)
    {
      return left.Count == right.Count;
    }

    private static bool CheckStructuralEquality<K, V>(Dictionary<K, V> left, Dictionary<K, V> right)
    {
      return CheckCount<K, V>(left, right) && CheckSequence<K, V>(left, right);
    }

    public static bool AreEqual<K, V>(Dictionary<K, V> left, Dictionary<K, V> right)
    {
      foreach (var key in left.Keys)
      {
        if (left[key] is IList && right[key] is IList)
        {
          var l = JsonConvert.SerializeObject(left[key]);
          var r = JsonConvert.SerializeObject(right[key]);
          if (l != r)
            return false;
        }
        else
        {
          if (DictionaryEntryTest<K, V>(left, right, key) == false)
            return false;
        }
      }

      return true;
    }

    //BUGFIX: Issues comparing Address which was a JObject
    public static bool DictionaryEntryTest<K, V>(Dictionary<K, V> left, Dictionary<K, V> right, K key)
    {
      if (left[key] == null && right[key] == null) return true;
      if (left[key] == null || right[key] == null) return false;

      if (left[key] is JObject || right[key] is JObject)
        return JObject.DeepEquals(left[key] as JObject, right[key] as JObject);
      else
        return (left[key]).Equals(right[key]);

    }
  }
}
