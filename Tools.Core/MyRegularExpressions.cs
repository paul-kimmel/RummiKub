using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tools
{
	public static class MyRegularExpressions
	{

		public static readonly Regex NotDigits = new Regex("[^0-9]", RegexOptions.Compiled);
		public static string GetFormattedPhoneNumber(string number)
		{
			if (string.IsNullOrWhiteSpace(number) || IsDigits(number, 7, 10, 11) == false) return number;

			long digits = 0L;
			return long.TryParse(number, out digits) ? string.Format(GetPhoneNumberFormatString(number.Length), digits) : number;
		}

		public static string GetPhoneNumberFormatString(int length) => length switch
		{
			11 => "{0:# (###) ###-####}",
			10 => "{0:(###) ###-####}",
			7 => "{0:###-####}",
			_ => "{0}"
		};


		public static bool IsPhoneNumber(string number)
		{
			return IsDigits(number, 7, 10, 11);
		}

		public static bool IsZipCode(string number)
		{
			return IsDigits(number, 5, 9);
		}

		const string PHONE_NUMBER = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
		public static readonly Regex phoneNumber = new Regex(PHONE_NUMBER, RegexOptions.Singleline | RegexOptions.Compiled);

		public static bool IsPhoneMatch(string number)
		{
			return string.IsNullOrWhiteSpace(number) == false && phoneNumber.Match(number).Length > 0;
		}

		public static bool IsDigits(string number, params int[] args)
		{
			if (number == null || args == null || args.Length == 0) return false;

			var result = NotDigits.Replace(number, "");
			if (string.IsNullOrWhiteSpace(result)) return false;
			return result.Length.In(args);
		}

		const string URL = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
		public static readonly Regex url = new Regex(URL, RegexOptions.Singleline | RegexOptions.Compiled);

		public static bool IsUrlMatch(string text)
		{
			return string.IsNullOrWhiteSpace(text) == false && url.Match(text).Success;
		}

		const string USERNAME = @"^[a-zA-Z0-9]+([._]?[a-zA-Z0-9]+)*$";
		public static readonly Regex username = new Regex(USERNAME, RegexOptions.Singleline | RegexOptions.Compiled);

		public static bool IsUserNameMatch(string text)
		{
			return string.IsNullOrWhiteSpace(text) == false && username.Match(text).Success;
		}

	}
}
