using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RummiKub.GamePlay;
using System.Diagnostics;

namespace RummiKub.Docker.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class Cards : ControllerBase
  {
    [HttpGet]
    public string GetCard(string name)
    {
      try
      {
        return CardFactory.GetCard(name);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
        return string.Empty;
      }

    }


  }
}
