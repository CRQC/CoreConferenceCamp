using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IConfiguration config;

        public OperationsController(IConfiguration config)
        {
            this.config = config;
        }

        [HttpOptions("realodconfig")]
        public ActionResult<bool> ReloadConfig() {

            try
            {
                var root = (IConfiguration)config;
                root.GetReloadToken();
                return true;
            
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
