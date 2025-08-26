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
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Diagnostics;

namespace Tools
{
  public class TrackProcessingTime
  {
    private class MyStopwatch : Stopwatch
    {

      public MyStopwatch()
      {
        SetName();
      }

      private void SetName()
      {
        const int DESIGNED_FOR_FRAME_DEPTH = 3;
        StackTrace trace = new StackTrace();
        Name = string.Format("{0}.{1}", trace.GetFrame(DESIGNED_FOR_FRAME_DEPTH).GetMethod().ReflectedType.Name,
              trace.GetFrame(DESIGNED_FOR_FRAME_DEPTH).GetMethod().Name);
      }

      private TextWriter writer = null;
      public MyStopwatch(TextWriter writer)
      {
        this.writer = writer;
        SetName();
      }

      public string Name { get; set; }

      private string GetReportText()
      {
        return string.Format("{0} took {1} milliseconds to complete", Name, ElapsedMilliseconds);
      }

      public void Report()
      {
        if (writer != null)
          writer.WriteLine(GetReportText());
        else
          MetaDumper.MyTrace(GetReportText());
      }
    }

    private static Stack<MyStopwatch> watches = new Stack<MyStopwatch>();

    public static void Start()
    {
      MyStopwatch watch = new MyStopwatch();
      watch.Start();
      watches.Push(watch);
    }

    public static double ElapsedTimeInSeconds()
    {
      return watches.Peek().Elapsed.TotalSeconds;
    }

    public static double StopWithTimeInSeconds()
    {
      MyStopwatch watch = watches.Pop();
      watch.Stop();
      return watch.Elapsed.TotalSeconds;
    }

    public static void Stop()
    {
      MyStopwatch watch = watches.Pop();
      watch.Stop();
      watch.Report();
    }
  }
}
