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
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class AssertionRunner<T>
  {
    public AssertionRunner()
    {
      Result = new ValidationResult();
    }
    public ValidationResult Result { get; set; }

    public static ValidationResult RunAll(Pair<T> pair, params InlineValidator<Pair<T>>[] tests)
    {
      var o = new AssertionRunner<T>();
      o.Run(pair, tests);
      return o.Result;
    }

    public void Run<U>(Pair<U> pair, params InlineValidator<Pair<U>>[] tests)
    {
      foreach (var action in tests)
      {
        var result = action.Validate(pair);
        foreach (var item in result.Errors)
          Result.Errors.Add(item);
      }
    }

    public static ValidationResult RunAll<U>(U target, params InlineValidator<U>[] tests)
    {
      var o = new AssertionRunner<T>();
      o.Run(target, tests);
      return o.Result;
    }

    public void Run<U>(U target, params InlineValidator<U>[] tests)
    {
      foreach (var action in tests)
      {
        var result = action.Validate(target);
        foreach (var item in result.Errors)
          Result.Errors.Add(item);
      }
    }
  }
}
