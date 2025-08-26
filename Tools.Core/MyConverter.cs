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
using System.ComponentModel;

namespace Tools
{
  public static class MyConverter
  {

    public static Nullable<T> GetValue<T>(object value) where T : struct
    {
      TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
      try
      {
        if (converter is DateTimeConverter)
        {
          var o = converter.ConvertFrom(value);
          return (DateTime)o == DateTime.MinValue ? null : (T?)o;
        }

        if (converter is GuidConverter)
        {
          return (Nullable<T>)converter.ConvertFrom(new Guid(value.ToString()).ToString());
        }

        return (Nullable<T>)converter.ConvertFrom(value);
      }
      catch (Exception ex)
      {
        MetaDumper.MyTrace(ex.Message);
        return null;
      }
    }

    public static string GetStringValue<T>(T? value) where T : struct
    {
      if (value.HasValue == false) return "";

      if (typeof(T) == typeof(DateTime))
        return Convert.ToDateTime(value.Value).ToShortDateString();
      else
        return value.ToString();
    }

    public static string ByteArrayToHexString(byte[] bytes)
    {
      var o = BitConverter.ToString(bytes);
      return string.Format("0x{0}", o.Replace("-", ""));
    }
  }
}
