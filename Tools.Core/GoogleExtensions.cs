using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geocoding.Google;

namespace Tools.Core
{
  public static class GoogleExtensions
  {
    public static string GetAddressLine1(this GoogleAddress o)
    {
      return string.Format("{0} {1}", o.GetStreetNumber(), o.GetRoute());
    }

    public static string GetAddressLine2(this GoogleAddress o)
    {
      return o.GetSubpremise();
    }

    public static string GetFormattedAddress(this GoogleAddress o)
    {
      return o.FormattedAddress;
    }

    public static string GetStreetNumber(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.StreetNumber);
    }

    public static string GetRoute(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.Route);
    }

    public static string GetSubpremise(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.Subpremise);
    }

    public static string GetCity(this GoogleAddress o)
    {
      var s = o.GetComponentString(GoogleAddressType.Neighborhood);
      return string.IsNullOrEmpty(s) ? o.GetComponentString(GoogleAddressType.Locality) : s;
    }

    public static string GetCounty(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.AdministrativeAreaLevel2);
    }

    public static string GetState(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.AdministrativeAreaLevel1);
    }

    public static string GetPostalCode(this GoogleAddress o)
    {
      return o.GetComponentString(GoogleAddressType.PostalCode);
    }

    public static string GetComponentString(this GoogleAddress o, GoogleAddressType type)
    {
      try
      {
        switch (type)
        {
          case GoogleAddressType.AdministrativeAreaLevel2:
          case GoogleAddressType.AdministrativeAreaLevel1:
            return o.Components.First(x => x.Types.Length > 0 ? x.Types[0] == type : false).ShortName;

          default:
            return o.Components.First(x => x.Types.Length > 0 ? x.Types[0] == type : false).LongName;
        }

      }
      catch
      {
        return "";
      }
    }

    public static double GetLatitude(this GoogleAddress o)
    {
      return o.Coordinates.Latitude;
    }

    public static double GetLongitude(this GoogleAddress o)
    {
      return o.Coordinates.Longitude;
    }

  }
}
