using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Routing;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;

namespace CoreCodeCamp.Controllers
{
    [ApiController]
    [Route("api/camps/{moniker}/talks/")]
    public class TalksController: ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;
        private readonly LinkGenerator _linkGenerator;

        
        public TalksController(ICampRepository repository, IMapper mapper,  LinkGenerator   linkGenerator )
        {
            _repository = repository;
            _mapper = mapper;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var talks = await _repository.GetTalksByMonikerAsync(moniker);
                return _mapper.Map<TalkModel[]>(talks);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TalkModel>> Get(string moniker, int id)
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id);
                return _mapper.Map<TalkModel>(talk);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel talkModel)
        {
            try
            {
                var camp = await _repository.GetCampAsync(moniker);
                if (camp == null)
                {
                    return BadRequest("Camp does not exist");
                }

                var talk = _mapper.Map<Talk>(talkModel);
                talk.Camp = camp;
                
                if (talkModel.Speaker == null) return BadRequest("Speaker ID is required");
                var speaker = await _repository.GetSpeakerAsync(talkModel.Speaker.SpeakerId);
                if (speaker == null) return BadRequest("Speaker could not be found");

                talk.Speaker = speaker;

                _repository.Add(talk);

                if (await _repository.SaveChangesAsync())
                {
                    var url = _linkGenerator.GetPathByAction(HttpContext,
                        "Get",
                        values: new { moniker, id = talk.TalkId });

                    return Created(url, _mapper.Map<TalkModel>(talk));
                }
                else
                {
                    return BadRequest("Failed to save new Talk");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to get Talks");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int id, TalkModel model) 
        {
            try
            {
                var talk = await _repository.GetTalkByMonikerAsync(moniker, id, true);
                if (talk == null)
                {
                    return NotFound("Could not fin the talk ");
                }

                if (model.Speaker != null)
                {
                    var speaker = await _repository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if (speaker != null)
                    {
                        talk.Speaker = speaker;
                    }
                }


                _mapper.Map(model, talk);

                if (await _repository.SaveChangesAsync())
                {
                    return _mapper.Map<TalkModel>(talk);
                }
                else
                {
                    return BadRequest("error on request");
                }



            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to put Talks");
            }
        
        }

    }
}
