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
  public class MetaAttribute : Attribute
  {
    public MetaAttribute(object ID, string description)
    {
      this.Description = description;
      this.ID = ID;
    }

    public string Description { get; private set; }
    public object ID { get; private set; }

    public bool IsGuid { get { return GetGuid() != Guid.Empty; } }
    public bool IsDecimal { get { return GetDecimal() != 0M; } }

    public Guid GetGuid()
    {
      Guid result;
      if (Guid.TryParse(ID.ToString(), out result))
        return result;

      return Guid.Empty;
    }

    public decimal GetDecimal()
    {
      decimal result;
      if (decimal.TryParse(ID.ToString(), out result))
        return result;

      return 0M;
    }
  }

}
