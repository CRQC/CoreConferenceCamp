using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase

    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper  mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks =  false) 
        {
            try
            {

                var results = await _repository.GetAllCampsAsync(includeTalks);
                return _mapper.Map<CampModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }


        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate,  bool includeTalks = false)
        {
            try
            {

                var results = await _repository.GetAllCampsByEventDate(theDate, includeTalks );

                if (!results.Any())
                {
                    return NotFound();
                }

                return _mapper.Map<CampModel[]>(results);
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error");
            }
        }


        public async Task<ActionResult<CampModel>> Post([FromBody] CampModel model)
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database error, on Post method (CampsController)");
            }
        
        
        
        }

    }
}
