using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tools.Atlas;

namespace Tools
{
  public class AtlasHelper
  {
    public static IConfigurationRoot GetIConfigurationRoot()
    {
      return new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }

    public static string AtlasSubscriptionKeyCore
    {
      get { return GetIConfigurationRoot().GetSection("AppSettings")["subscriptionKey"].ToString(); }
    }

		public static async Task<GeocodedAddress> GeocodeAddressAsync(string address)
		{
			try
			{
				var result = await GetResultAsync(address);
				var o = new GeocodedAddress();
				JsonConvert.PopulateObject(result, o);

				CheckUnpackError(o, result);
				return o;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}

		private static void CheckUnpackError(GeocodedAddress o, string result)
		{
			if (o == null || o.Summary == null)
			{
				var error = new GeocodeError();
				JsonConvert.PopulateObject(result, error);
				throw new Exception($"Code: {error.Error.Code}, Message: {error.Error.Message}");
			}
		}


		public static async Task<string> GetResultAsync(string address)
		{
			var url = $"https://atlas.microsoft.com/search/address/json?api-version=1.0&query={address}&subscription-key={AtlasHelper.AtlasSubscriptionKeyCore}";

			try
			{
				using (var client = new HttpClient())
				{
					using (var response = await client.GetAsync(url))
					{
						return await response.Content.ReadAsStringAsync();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw;
			}
		}
	}
}
