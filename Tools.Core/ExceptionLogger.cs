using System;
using System.Diagnostics;
using Tools;

namespace Tools
{
  public static class ExceptionLogger
  {
    
    public static void Log(this Exception ex)
    {
      Debug.WriteLine(ex.Message);
      EventLog.WriteEntry("Application", string.Format("{0}: {1}", MetaDumper.GetMethodName(3), ex.Message), EventLogEntryType.Error);
    }
  }
}
