using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
  public class Expressions
  {
    public static readonly string PhoneNumber = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$";
    public static readonly string Email = @"[-0-9a-zA-Z.+_]+@[-0-9a-zA-Z.+_]+\.[a-zA-Z]{2,4}";

    public static readonly string WebSite = @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";
    public static readonly string ZipCode = @"(\d{5})(\d{4})?";
    public static readonly string PostalCode = @"(\d{5})(\d{4})?";
    public static readonly string Month = @"1[012]|[1-9]";
    public static readonly string CVV = @"\d{3,4}";
  }
}
