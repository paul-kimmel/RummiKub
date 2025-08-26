#undef ENVDTE
#if ENVDTE
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
using EnvDTE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class RunningObjectTableHelper
  {

    [DllImport("ole32.dll")]
    public static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

    [DllImport("ole32.dll")]
    public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

    public static Hashtable GetRunningObjects(string key)
    {
      Hashtable result = new Hashtable();

      IntPtr fetched = IntPtr.Zero;
      IRunningObjectTable runningObjectTable;
      IEnumMoniker monikerEnumerator;
      IMoniker[] monikers = new IMoniker[1];

      GetRunningObjectTable(0, out runningObjectTable);
      runningObjectTable.EnumRunning(out monikerEnumerator);
      monikerEnumerator.Reset();

      while (monikerEnumerator.Next(1, monikers, fetched) == 0)
      {
        IBindCtx ctx;
        CreateBindCtx(0, out ctx);

        string runningObjectName;
        monikers[0].GetDisplayName(ctx, null, out runningObjectName);

        object runningObjectVal;
        runningObjectTable.GetObject(monikers[0], out runningObjectVal);

        if (runningObjectName.StartsWith(key))
          result[runningObjectName] = runningObjectVal;
      }

      result.Dump();

      return result;
    }

    public static string GetProjectRootFolder(string projectName)
    {
      var results = RunningObjectTableHelper.GetRunningObjects("!VisualStudio.DTE");

      IDictionaryEnumerator enumerator = results.GetEnumerator();
      while (enumerator.MoveNext())
      {
        _DTE ide = enumerator.Value as _DTE;

        foreach (var item in ide.Solution.Projects)
        {
          var project = item as Project;
          if (project.FullName.ToUpper().Contains(projectName.ToUpper()))
          {
            return Path.GetDirectoryName(project.FullName);
          }
        }
      }

      return "";
    }

  }
}
#endif