using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geocoding.Google;

namespace Tools.Core
{
  public class GeocodeHelper
  {
    public static IConfigurationRoot GetIConfigurationRoot()
    {
      return new Microsoft.Extensions.Configuration.ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
    }


    public static string GoogleApiKey
    {
      get { return ConfigurationManager.AppSettings["googleApiKey"]; }
    }

    public static string ConsumerKey
    {
      get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
    }

    public static string ConsumerSecret
    {
      get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
    }



    public static string GoogleApiKeyCore
    {
      get { return GetIConfigurationRoot().GetSection("AppSettings")["googleApiKey"].ToString(); }
    }

    [Description("Subscription Service. Requires Approval.")]
    public static Tuple<string, string, string, string, string, double, double> ResolveAddressCore_Google(string address)
    {
      try
      {
        var o = AddressResolver.Resolve((GoogleAddress)new GoogleGeocoder(GoogleApiKeyCore).GeocodeAsync(address).Result.First());
        return Tuple.Create(o.FormattedAddress, o.StreetAddress, o.City, o.State, o.PostalCode, o.Latitude, o.Longitude);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Tuple.Create<string, string, string, string, string, double, double>(address, "", "", "", "", 0.0, 0.0);
      }
    }

    [Description("Subscription Service. Requires Approval.")]
    public async static Task<Tuple<string, string, string, string, string, string, double, Tuple<double>>> ResolveAddressAsync(string address)
    {
      try
      {
        var google = new GoogleGeocoder(GoogleApiKeyCore);
        var geocode = await google.GeocodeAsync(address);

        var o = AddressResolver.Resolve(geocode.First());
        return Tuple.Create(o.FormattedAddress, o.StreetAddress, o.City, o.State, o.PostalCode, o.Country, o.Latitude, o.Longitude);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Tuple.Create(address, "", "", "", "", "", 0.0, 0.0);
      }
    }

    public static Tuple<string, string, string, string, string, double, double> ResolveAddress(string address)
    {
      try
      {
        var o = AddressResolver.Resolve((GoogleAddress)new GoogleGeocoder(GoogleApiKey).GeocodeAsync(address).Result.First());
        return Tuple.Create(o.FormattedAddress, o.StreetAddress, o.City, o.State, o.PostalCode, o.Latitude, o.Longitude);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Tuple.Create<string, string, string, string, string, double, double>(address, "", "", "", "", 0.0, 0.0);
      }
    }

    public static Tuple<string, string, string, string, string, double, double> ResolveAddressCore(string address)
    {
      try
      {
        var o = AddressResolver.Resolve((GoogleAddress)new GoogleGeocoder(GoogleApiKeyCore).GeocodeAsync(address).Result.First());
        return Tuple.Create(o.FormattedAddress, o.StreetAddress, o.City, o.State, o.PostalCode, o.Latitude, o.Longitude);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return Tuple.Create<string, string, string, string, string, double, double>(address, "", "", "", "", 0.0, 0.0);
      }
    }
  }

}
