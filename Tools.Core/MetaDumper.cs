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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Tools
{
  public static class MetaDumper
  {
    public static void Dump(this DataSet data, TextWriter writer)
    {
      foreach (DataTable table in data.Tables)
        table.Dump(writer);
    }

    public static void DumpSchema(this DataSet data, TextWriter writer)
    {
      foreach (DataTable table in data.Tables)
        table.DumpSchema(writer);
    }

    public static void DumpSchema(this DataTable table, TextWriter writer)
    {
      writer.WriteLine(table.TableName);
      writer.WriteLine(new string('-', 40));
      foreach (DataColumn column in table.Columns)
        writer.WriteLine("Column Name: {0}, Data Type: {1}", column.ColumnName, column.DataType);
      writer.WriteLine(new string('*', 40));
    }

    public static void Dump(this DataTable table, TextWriter writer)
    {
      writer.WriteLine(table.TableName);
      writer.WriteLine(new string('-', 40));
      foreach (DataRow row in table.Rows)
        row.Dump(writer);
      writer.WriteLine(new string('*', 40));
    }

    public static void Dump(this DataTable table, int count, TextWriter writer)
    {
      try
      {
        DataTable clone = table.Clone();
        for (int i = 0; i < table.Rows.Count && i < count; i++)
          clone.ImportRow(table.Rows[i]);
        clone.Dump(writer);
      }
      catch (Exception ex)
      {
        MetaDumper.MyTrace(ex.Message);
      }
    }

    public static void Dump(this IEnumerable<DataRow> list, TextWriter writer)
    {
      foreach (DataRow row in list)
        row.Dump(writer);
    }

    public static void Dump(this DataRow row, TextWriter writer)
    {
      foreach (DataColumn column in row.Table.Columns)
        writer.WriteLine(string.Format("{0} : {1}", column.ColumnName, row[column]));
      writer.WriteLine();
    }

#if ASP_NET
    public static void Dump(this IHierarchicalEnumerable list, TextWriter writer)
    {
      foreach (var item in list)
        (item as IHierarchyData).Dump(writer);
    }

    public static void Dump(this IHierarchyData data, TextWriter writer)
    {
      data.Item.Dump(writer, true);
      if (data.HasChildren)
        data.GetChildren().Dump(writer);
    }
#endif

    public static void Dump(this IEnumerable list)
    {
      list.Dump(Console.Out);
    }

    public static void Dump(this IEnumerable list, TextWriter writer)
    {
      list.Dump(writer, false);
    }

    public static void Dump(this IEnumerable list, TextWriter writer, bool sorted)
    {
      if (list is Enum)
        DumpEnum(list as Enum, writer);
      else
        if (list is string)
        DumpString(list as string, writer);
      else
        foreach (object o in list)
          Dump(o, writer, sorted);
    }

    public static void Dump(this List<ExpandoObject> list)
    {
      list.Dump(Console.Out);
    }

    public static void Dump(this List<ExpandoObject> list, TextWriter writer)
    {
      (list as IEnumerable<ExpandoObject>).Dump(writer);
    }

    public static void Dump(this IEnumerable<ExpandoObject> list)
    {
      list.Dump(Console.Out);
    }

    public static void Dump(this IEnumerable<ExpandoObject> list, TextWriter writer)
    {
      foreach (dynamic o in list)
        (o as ExpandoObject).Dump(writer);
    }

    public static void Dump(this ExpandoObject item)
    {
      Dump(item, Console.Out);
    }

    public static void Dump(this ExpandoObject item, TextWriter writer)
    {
      DynamicMetaObject o = (item as IDynamicMetaObjectProvider).GetMetaObject(Expression.Constant(item));
      foreach (var name in o.GetDynamicMemberNames())
      {
        try
        {
          writer.WriteLine("{0}: {1}", name, ((IDictionary<string, object>)item)[name]);
        }
        catch
        {
          writer.WriteLine("{0}: {1}", name, o.HasValue ? o.Value.ToString() : "");
        }
      }
    }

    public static void Dump(this object o, TextWriter writer, bool sorted)
    {
      if (o is ValueType)
      {
        DumpString(o.ToString(), writer);
      }
      if (o is Enum)
      {
        DumpEnum(o, writer);
      }
      else
      if (o is string)
      {
        DumpString(o, writer);
      }
      else
      {
        PropertyInfo[] infos = GetProperties(o, sorted);
        foreach (PropertyInfo info in infos)
        {
          object value = info.TryGetValue(o);
          if (value != null && value.GetType().IsArray && ((Array)value).Length > 0)
            value = GetArrayValues((Array)value, "{0:x}, {1:x}");

          writer.WriteLine(string.Format("{0}: {1}", info.Name, value ?? "unk"));
        }

        writer.WriteLine();
      }

    }

    private static void DumpEnum(object o, TextWriter writer)
    {
      writer.WriteLine(Enum.GetName(o.GetType(), o));
      writer.WriteLine();
    }

    private static void DumpString(object o, TextWriter writer)
    {
      writer.WriteLine(o.ToString());
      writer.WriteLine();
    }

    private static PropertyInfo[] GetProperties(object o, bool sorted)
    {
      return sorted ? GetSortedProperties(o) : GetProperties(o);
    }

    private static PropertyInfo[] GetProperties(object o)
    {
      return o.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DumpFreeAttribute)) == null).ToArray();
    }

    private static PropertyInfo[] GetSortedProperties(object o)
    {
      return GetProperties(o).OrderBy(x => x.Name).ToArray<PropertyInfo>();
    }

    public static void DumpDynamic(dynamic o, TextWriter writer)
    {
      if (o is IEnumerable)
        (o as IEnumerable).Dump(writer);
    }

    private static readonly string line = new string('-', 40);
    public static void WriteHeader(string title)
    {
      Console.WriteLine(line);
      Console.WriteLine(title);
      Console.WriteLine(line);
    }

    public static void Dump(this object o, string[] propertiesToExpand)
    {
      //WriteHeader(o.GetType().Name);
      //o.Dump();

      foreach (var item in propertiesToExpand)
      {
        WriteHeader(item);
        try
        {
          var info = o.GetType().GetProperty(item);
          var value = info.TryGetValue(o);
          if (value != null)
            value.Dump();
        }
        catch (Exception ex)
        {
          Debug.WriteLine(ex.Message);
        }
      }
    }

    public static void Dump(this object o)
    {
      Dump(o, Console.Out);
    }

    public static void Dump(this object o, TextWriter writer)
    {
      Dump(o, writer, false);
    }

    public static string GetArrayValuesSkippingEmpties(IEnumerable value, string mask)
    {
      if (value == null) return "";
      try
      {
        return (string)(from object v in value
                        where v != null && v.ToString().Length > 0
                        select v.ToString()).Aggregate((s, t) => string.Format(mask, s, t));
      }
      catch
      {
        return "";
      }
    }

    public static string GetArrayValues(IEnumerable value, string mask)
    {
      if (value == null) return "";
      try
      {
        return (string)(from object v in value
                        select v.ToString()).Aggregate((s, t) => string.Format(mask, s, t));
      }
      catch
      {
        return "";
      }
    }

    public static void MyTrace(IEnumerable list)
    {
      StackFrame frame = new StackTrace().GetFrame(2);
      Trace.WriteLine(string.Format("{0}{1}(", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name));
      Trace.WriteLine(String.Format("{0}", GetArrayValues(list, "{0}, {1}")));
      Trace.WriteLine(")");
    }

    public static void MyTrace()
    {
      StackFrame frame = new StackTrace().GetFrame(2);
      Trace.WriteLine(string.Format("{0}.{1}[{2}]", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name, frame.GetFileName()));
    }

    public static string GetMethodName()
    {
      return GetMethodName(2);
    }


    public static string GetMethodName(int index)
    {
      try
      {
        StackFrame frame = new StackTrace().GetFrame(index);
        return frame.GetMethod().DeclaringType.FullName;
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return "unknown";
      }
    }


    public static void MyTrace(string content)
    {
      StackFrame frame = new StackTrace().GetFrame(2);
      try
      {
        Trace.WriteLine(string.Format("{0}.{1}[{2}]({3})", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name, frame.GetFileName(), content));
      }
      catch (Exception ex)
      {
        Trace.WriteLine(string.Format("{0}: {1}", content, ex.Message));
      }
    }

    public static void MyTrace(string format, params object[] args)
    {
      StackFrame frame = new StackTrace().GetFrame(2);
      Trace.WriteLine(string.Format("{0}.{1}[{2}]({3})", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name, frame.GetFileName(), string.Format(format, args)));
    }

    public static void MyTrace(ValueType content)
    {
      StackFrame frame = new StackTrace().GetFrame(2);
      Trace.WriteLine(string.Format("{0}.{1}[{2}]({3})", frame.GetMethod().DeclaringType.FullName, frame.GetMethod().Name, frame.GetFileName(), content));
    }

    public static string GetState(this IEnumerable list)
    {
      StringBuilder builder = new StringBuilder();
      foreach (object o in list)
        builder.Append(o.GetState());
      return builder.ToString();
    }

    public static string GetState(this object o)
    {
      StringBuilder builder = new StringBuilder();
      PropertyInfo[] infos = o.GetType().GetProperties().Where(x => x.GetCustomAttribute(typeof(DumpFreeAttribute)) == null).ToArray();
      foreach (PropertyInfo info in infos)
      {
        object value = info.GetValue(o, null);
        if (value != null && value.GetType().IsArray)
          value = GetArrayValues((Array)value, "{0:x}, {1:x}");
        builder.AppendFormat(string.Format("{0}: {1},", info.Name, value ?? "unknown"));
      }
      return builder.ToString();
    }

    public static string GetState(this List<ExpandoObject> list)
    {
      StringBuilder builder = new StringBuilder();
      foreach (dynamic o in list)
        builder.Append((o as ExpandoObject)
          .GetState());
      return builder.ToString();
    }

    public static string GetState(this ExpandoObject item)
    {
      StringBuilder builder = new StringBuilder();
      DynamicMetaObject o = (item as System.Dynamic.IDynamicMetaObjectProvider).GetMetaObject(Expression.Constant(item));
      foreach (var name in o.GetDynamicMemberNames())
      {
        try
        {
          builder.AppendFormat("{0}: {1},", name, ((IDictionary<string, object>)item)[name]);
        }
        catch
        {
          builder.AppendFormat("{0}: {1},", name, o.HasValue ? o.Value.ToString() : "");
        }
      }
      return builder.ToString();
    }

    public static XElement GetXmlState(this IEnumerable list)
    {
      return new XElement("List",
      from object o in list
      select GetXmlState(o));
    }

    public static XElement GetXmlState(this object o)
    {
      PropertyInfo[] infos = o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttribute(typeof(DumpFreeAttribute)) == null).ToArray();
      XElement element = new XElement(o.GetType().Name,
      from info in infos
      where info.Name != "Item"
      select new XElement(info.Name, GetValue(info, o)));
      return element;
    }

    public static object GetValue(PropertyInfo p, object o)
    {
      try
      {
        if (o == null || o.ToString() == "null") return "null";
        var result = p.TryGetValue(o);
        if (result is IEnumerable && result is string == false)
        {
          return GetXmlState(result as IEnumerable);
        }
        if (p.PropertyType.IsClass && p.PropertyType.IsPrimitive == false && result != null
        && p.PropertyType.Name != p.ReflectedType.Name && result is string == false)
        {
          //Debugger.Break();
          return GetXmlState(result);
        }
        return result;
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        return "blank";
      }
    }

    /// <summary>
    /// Show me what is loop and where
    /// </summary>
    /// <param name="writer"></param>
    public static void DumpCallStackLoops(TextWriter writer)
    {
      //Debugger.Break();
      StackTrace trace = new StackTrace();
      var loops = from StackFrame frame in trace.GetFrames()
                  group frame by frame.GetMethod().Name into methodGroups
                  select methodGroups;
      writer.WriteLine("\r\n=========== BEGIN DUMPING REPEATED METHOD CALLS");
      foreach (var group in loops)
      {
        try { MyDebug.Trace("{0}({1})", group.ToArray()[0].GetMethod().Name, group.Count()); }
        catch { }
        foreach (var frame in group)
          writer.Write("\t{0}", frame);
        writer.WriteLine();
      }
      writer.WriteLine("=========== END DUMPING REPEATED METHOD CALLS\r\n");

    }

    /// <summary>
    /// Dump the current callstack
    /// </summary>
    /// <param name="writer"></param>
    public static void DumpCallStack(TextWriter writer)
    {
      Debug.Assert(writer != null);
      if (writer == null) return;
      writer.WriteLine("\r\n========BEGIN STACK DUMP");
      //Debugger.Break();
      StackTrace trace = new StackTrace();
      foreach (StackFrame frame in trace.GetFrames())
        writer.Write("\t{0}", frame);
      writer.WriteLine("===========END STACK DUMP\r\n");
    }

    public static void AddMarker(TextWriter writer)
    {
      try
      {
        StackTrace trace = new StackTrace();
        var method = trace.GetFrames()[1].GetMethod();
        Console.Write(method.Name);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    public static void AddMarker()
    {
#if DEBUG
      try
      {
        StackTrace trace = new StackTrace();
        var method = trace.GetFrames()[1].GetMethod();
        Console.Write(method.Name);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
#endif
    }

    public static void AddMarkers()
    {
      try
      {
        StackTrace trace = new StackTrace();
        List<string> list = new List<string>();

        foreach (StackFrame frame in trace.GetFrames())
          list.Add(frame.GetMethod().Name);

        Console.Write(JsonConvert.SerializeObject(list));
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }
  }
}
