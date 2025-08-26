using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Tools.Core
{
	public static class CastExtensions
	{
		public static List<T> Cast<T>(this List<ExpandoObject> source)
		where T : new()
		{
			var list = new List<T>();

			foreach (ExpandoObject o in source)
			{
				list.Add(o.Cast<T>());
			}

			return list;
		}

		public static T Cast<T>(this ExpandoObject source)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;

			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), settings);
		}

		public static T Cast<T>(this KeyValuePair<string, object> source)
		{
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
		}
	}
}
