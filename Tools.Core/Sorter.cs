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
using System.ComponentModel;
using System.Linq;
using System.Text;


namespace Tools
{
  public class Sorter<T> : IComparer<T>
  {
    public PropertyDescriptor Descriptor { get; protected set; }
    public SortDirection Direction { get; protected set; }

    public Sorter(PropertyDescriptor descriptor, SortDirection direction)
    {
      this.Descriptor = descriptor;
      this.Direction = direction;
    }

    public virtual int Compare(T x, T y)
    {
      return CompareTo(Descriptor.GetValue(x), Descriptor.GetValue(y), Direction);
    }

    protected virtual int CompareTo(object x, object y, SortDirection sortOrder = SortDirection.Ascending)
    {
      int result = 0;
      if (x == null && y == null) return result;

      if (x is IComparable)
        result = ((IComparable)x).CompareTo(y);
      else if (y is IComparable)
        result = ((IComparable)y).CompareTo(x);
      else if (x.Equals(y) == false)
        result = x.ToString().CompareTo(y.ToString());

      return result * (int)sortOrder;
    }
  }
}

