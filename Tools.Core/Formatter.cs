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
using System.Text.RegularExpressions;
using System.Collections;

namespace Tools
{
	public static class Formatter
	{
		public static string GetFormatedZipCode(string zipCode)
		{
			if (Regex.IsMatch(zipCode, @"^\d{10}$")) return zipCode;
			if (Regex.IsMatch(zipCode, @"^\d{9}$")) return Regex.Replace(zipCode, @"(\d{5})(\d{4})", "$1-$2");
			return zipCode;
		}

		public static string GetArrayValues(this IEnumerable list, string mask)
		{
			return MetaDumper.GetArrayValues(list, mask);
		}

		public static string GetFormattedPhoneNumber(string number)
		{
			if (string.IsNullOrWhiteSpace(number) || Tools.MyRegularExpressions.IsDigits(number, 7, 10, 11) == false) return number;

			long digits = 0L;
			return long.TryParse(number, out digits) ? string.Format(GetPhoneNumberFormatString(number.Length), digits) : number;
		}

		public static string GetPhoneNumberFormatString(int length)
		{
			switch (length)
			{
				case 11: return "{0:# (###) ###-####}";
				case 10: return "{0:(###) ###-####}";
				case 7: return "{0:###-####}";
				default: return "{0}";
			}
		}

	}
}