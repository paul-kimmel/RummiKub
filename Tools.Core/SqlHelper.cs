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
using System.Threading.Tasks;

namespace Tools
{
  public class SqlHelper
  {
    /// <summary>
    /// Given: [User], UserName, 'pkimmel'
    /// When: username = 'pkimmel', UserName = 'pkimmel1', ..., UserName = 'pkimmel9', UserName = 'pkimmel10'
    /// Return 'pkimmel10'
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="fieldname"></param>
    /// <param name="characterPrefix"></param>
    /// <returns></returns>
    public static string GetSqlOrderByNumericSuffix(string tableName, string fieldname, string characterPrefix)
    {
      string sql = @"SELECT TOP 1 {0},
		                 CASE WHEN ISNUMERIC(SUBSTRING({0}, PATINDEX('%[0-9]%', {0}), LEN({0}))) = 1
                     THEN CONVERT(INT, SUBSTRING({0}, PATINDEX('%[0-9]%', {0}), LEN({0})))
		                 ELSE -1
	                   END	AS Digits
	                   FROM [{1}] WHERE username LIKE '{2}%' ORDER BY Digits DESC";

      return string.Format(sql, fieldname, tableName, characterPrefix);
    }
  }
}
