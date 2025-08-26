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

namespace Tools
{
  public class PLSQL
  {
    public static string Unhork(string PLSQLGuidString)
    {
      //0123 4567 8901  2345  6789  012345678901
      //10D0 5576 325A  53F7  E054  0021280E4A2E
      return (PLSQLGuidString.Substring(6, 2) + PLSQLGuidString.Substring(4, 2) +
        PLSQLGuidString.Substring(2, 2) + PLSQLGuidString.Substring(0, 2) +
        PLSQLGuidString.Substring(10, 2) + PLSQLGuidString.Substring(8, 2) +
        PLSQLGuidString.Substring(14, 2) + PLSQLGuidString.Substring(12, 2) +
        PLSQLGuidString.Substring(16)).ToUpper();
    }
  }
}
