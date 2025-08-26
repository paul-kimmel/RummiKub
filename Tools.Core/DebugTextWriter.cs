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
using System.IO;
using System.Diagnostics;

namespace Tools
{
  public class DebugTextWriter : TextWriter
  {
    public override Encoding Encoding
    {
      get { return Encoding.UTF8; }
    }

    //Required 
    public override void Write(char value)
    {
      Debug.Write(value);
    }

    //Added for efficiency 
    public override void Write(string value)
    {
      Debug.Write(value);
    }

    //Added for efficiency 
    public override void WriteLine(string value)
    {
      MetaDumper.MyTrace(value);
    }
  } 

}
