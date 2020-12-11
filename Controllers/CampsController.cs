using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCamps() 
        {
            //if (false)
            //{
            //    return BadRequest("Bad stuff happends");
            //}

            return Ok(new { Moniker = "ATL2018", Name = "Atlanta Code Camp" });
        }

    }
}
