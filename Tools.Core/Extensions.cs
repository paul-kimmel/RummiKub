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
using System.Linq;
using System.Linq.Expressions;

namespace Tools
{
  public static class Extensions
  {
    public static bool In<T>(this T target, T[] set)
    {
      if (set == null) return false;
      return set.Contains(target);
    }
    public static bool InSet<T>(this T target, params T[] set)
    {
      if (set == null) return false;
      return set.Contains(target);
    }

    public static void Guard(this object o, string message)
    {
      if (o == null)
        throw new ArgumentNullException("ploff");
    }

    public static void Guard(this string o, string message)
    {
      if (string.IsNullOrEmpty(o))
        throw new ArgumentNullException(message);
    }
    

    public static string SplitPascalCase(this string input)
    {
      if (string.IsNullOrEmpty(input)) return input;

      string[] items = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < items.Length; i++)
        items[i] = PascalCase(items[i]);

      return string.Join(" ", items);
    }

    public static string PascalCase(string input)
    {
      if (string.IsNullOrEmpty(input)) return input;
      if (input.ToLower().In(new string[] { "us", "usa", "n", "s", "e", "w", "ne", "se", "sw", "nw", "mi" })) return input.ToUpper();
      return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
    }


    public static string GetPropertyName(this object o, LambdaExpression expression)
    {
      MemberExpression body = (MemberExpression)expression.Body;
      return body.Member.Name;
    }

    public static string GetPropertyName<T>(this T o, Expression<Func<T>> expression)
    {
      var body = expression.Body as MemberExpression;
      if (body == null)
      {
        body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
      }
      return body.Member.Name;
    }

    public static string GetPropertyName<T>(this T o, Expression<Func<object, object>> expression)
    {
      var body = expression.Body as MemberExpression;
      if(body == null)
      {
        body = ((UnaryExpression)expression.Body).Operand as MemberExpression;
      }
      return body.Member.Name;
    }

    public static object GetPropertyValue<T>(this T o, LambdaExpression expression)
    {
      expression.Guard("expression cannot be null");
      var func = expression.Compile();
      return func.DynamicInvoke(o);
    }

    public static object GetPropertyValue<T>(this T o, Expression<Func<T, object>> expression)
    {
      expression.Guard("expression cannot be null");
      var func = expression.Compile();
      return func(o);
    }

    public static object GetPropertyValue<T>(this T o, string name)
    {
      Type t = o.GetType();
      return t.GetProperty(name).GetValue(o, null);
    }

    public static void SetPropertyValue<T>(this T o, string name, object value)
    {
      Type t = o.GetType();
      t.GetProperty(name).SetValue(o, value, null);
    }
  }

  public class Set<T>
  {
    protected T v;

    public Set(T value)
    {
      v = value;
    }

    public static bool operator ^(Set<T> target, T[] set)
    {
      return target.v.In(set);
    }

    public static implicit operator T(Set<T> set)
    {
      return set.v;
    }

    public static implicit operator Set<T>(T v)
    {
      return new Set<T>(v);
    }
  }
}
