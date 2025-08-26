﻿using System;
using System.Linq;

namespace Tools.Atlas
{
  public class Summary
  {
    public string Query { get; set; }
    public string QueryType { get; set; }
    public int QueryTime { get; set; }
    public int NumResults { get; set; }
    public int Offset { get; set; }
    public int TotalResults { get; set; }
    public int FuzzyLevel { get; set; }


  }
}
