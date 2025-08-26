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
  public static class StringExtensions
  {
    public static string IncrementNumberPart(this string s)
    {
      var suffix = Regex.Match(s, @"\d+$").Value;
      int value;

      if (Int32.TryParse(suffix, out value))
        return s.Replace(value.ToString(), (++value).ToString());
      else
        return string.Format("{0}1", s);
    }

    private static readonly Regex maskedPassword = new Regex(@"password=[^\s]+;?", RegexOptions.IgnoreCase);
    public static string MaskPassword(this string connectionString)
    {
      try
      {
        return maskedPassword.Replace(connectionString, "password=xxxxxxxx;");
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return connectionString;
      }
    }

    public static string Reverse(this string o)
    {
      if (string.IsNullOrWhiteSpace(o)) return o;

      var builder = new StringBuilder();
      Array.ForEach(o.ToCharArray(), x => {
        builder.Insert(0, x);
      });

      return builder.ToString();
    }

    public static string Reverse2(this string o)
    {
      if (string.IsNullOrWhiteSpace(o)) return o;

      var builder = new StringBuilder();

      for (int i = o.Length - 1; i >= 0; i--)
      {
        builder.Append(o[i]);
      }

      return builder.ToString();

    }

    public static string Reverse3(this string o)
    {
      if (string.IsNullOrWhiteSpace(o)) return o;

      return o.ToCharArray().Reverse().GetArrayValues("{0}{1}");
    }

    public static bool IsInEnum<T>(this string[] target) where T : struct
    {
      return target.Any(o => o.In(Enum.GetNames(typeof(T))));
    }

    public static T GetEnumValue<T>(this string[] target) where T : struct
    {
      foreach (var item in target)
      {
        T result;
        if (Enum.TryParse<T>(item, true, out result))
        {
          return result;
        }
      }

      throw new ArgumentOutOfRangeException(string.Join(" ", target));

    }
  }
}


