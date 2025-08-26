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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Xunit;

namespace Tools
{
  public static class TestHelper
  {
    private static readonly Hashtable hash = new Hashtable();

    public static void Given(this Object o)
    {
      Console.WriteLine($"Given: {o}");
      hash.Add(o.GetHashCode(), o);
    }

    public static void Given(this Object o, object value, TextWriter writer)
    {
      if (writer != null)
        writer.WriteLine($"Given: {value}");
      hash.Add(o.GetHashCode(), value);
    }


    public static void Given(this Object o, object value)
    {
      Given(o, value, Console.Out);
    }

    public static void Expect(this Object o)
    {
      o.Expect(true);
    }

    public static void Expect(this Object o, TextWriter writer)
    {
      o.Expect(true, writer);
    }

    public static void Expect(this Object o, object expected, TextWriter writer)
    {
      if (writer != null)
        writer.WriteLine($"Expect: {expected}");

      object _actual = hash[o.GetHashCode()];
      hash.Remove(o.GetHashCode());

      Assert.True((_actual == null && expected == null) || (_actual != null && _actual.Equals(expected)), String.Format("Given: {0}, Expect: {1}", _actual, expected));
    } 

    public static void Expect(this Object o, object expected)
    {
      Expect(o, expected, Console.Out);
    }

 
    public static object InvokeInstance(object o, string name, object[] args)
    {
      return o.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(o, args);
    }

    public static object InvokeStatic(object o, string name, object[] args)
    {
      return o.GetType().GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static).Invoke(o, args);
    }
  }

}
