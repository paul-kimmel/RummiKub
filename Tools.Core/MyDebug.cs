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
using System.Diagnostics;

namespace Tools
{
  public class MyDebug
  {
    public static void Assert(bool condition)
    {
      Debug.Assert(condition);
    }

    public static void Assert(bool condition, string message)
    {
      Debug.Assert(condition, message);
    }

    public static void Assert(bool condition, string message, string detailMessage)
    {
      Debug.Assert(condition, message, detailMessage);
    }

    public static void Assert(bool condition, string message, string detailMessageFormat, params object[] args)
    {
      Debug.Assert(condition, message, detailMessageFormat, args);
    }

    public static void Trace(object value)
    {
      Debug.WriteLine(value);
    }

    public static void Trace(string message)
    {
      Debug.WriteLine(message);
    }

    public static void Trace(string format, params object[] args)
    {
      Debug.WriteLine(format, args);
    }

#if DEBUG
    private static HashSet<string> trapMethods = null;
 
    protected static HashSet<string> TrapMethods
    {
      get
      {
        if(trapMethods == null)
          trapMethods = new HashSet<string>();
 
        return trapMethods;
      }
    }
 
    private static bool trapping = true;
 
    public static void PauseTrapping()
    {
      trapping = false;
    }
 
    public static void ResumeTrapping()
    {
      trapping = true;
    }
    
    public static void Trap()
    {
      TrapInternal("<no message>", GetTrappedMethodsName());
    }    
 
    public static void Trap(string message)
    {
      TrapInternal(message,  GetTrappedMethodsName());
    }
 
    private static void TrapInternal(string message, string trappedMethodsName)
    {
      if (!trapping) return;
     
      
      if (!TrapMethods.Contains(trappedMethodsName))
      {
        TrapMethods.Add(trappedMethodsName);
        Debug.Assert(false, string.Format("Trapped {0}: {1}", trappedMethodsName, message));
      }
    }
 
    private static string GetTrappedMethodsName()
    {
      // if we move this code we need to adjust the stack trace level
      StackTrace trace = new StackTrace();
      return string.Format("{0}.{1}", trace.GetFrame(2).GetMethod().ReflectedType.Name,
        trace.GetFrame(2).GetMethod().Name);
    }
 
    public static void ResetTraps()
    {
      trapMethods.Clear();
    }
#else
    public static void Trap(string message) { }
    public static void ResetTraps() { }
    public static void PauseTrapping() { }
    public static void ResumeTrapping() { }

#endif

  }
}
