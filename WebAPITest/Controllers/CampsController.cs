using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPITest.Data;
using WebAPITest.Data.Entities;
using WebAPITest.Models;

namespace WebAPITest.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : BaseController
    {
        private ICampRepository _repo;
        private ILogger<CampsController> _logger;
        private IMapper _mapper;

        public CampsController(ICampRepository repo, ILogger<CampsController> logger, IMapper mapper)
        {
            _repo = repo;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet()]
        public IActionResult Get1()
        {
            var camp = _repo.GetAllCamps();
            var addr = _repo.GetHashCode();

            // Anonmous Type
            // Ok -> 200
            return Ok(_mapper.Map<ICollection<CampModel>>(camp));
        }

        [HttpGet("[action]/{index}")]
        public IActionResult Get2(int index)
        {
            var camp = _repo.GetCamp(index);
            
            // Ok -> 200
            return Ok(_mapper.Map<CampModel>(camp));
        }

        [HttpGet("{index}", Name = "MyIndexGet")]
        public IActionResult Get3(int index, bool speacker)
        {
            try
            {
                Camp camp = null;
                if (speacker)
                {
                    camp = _repo.GetCampWithSpeakers(index);

                }
                else
                {
                    camp = _repo.GetCamp(index);
                }
                
                if (camp == null)
                    return NotFound($"Camp {index} is not found...");
                return Ok(_mapper.Map<CampModel>(camp));
            }
            catch
            {
                
            }

            return BadRequest();
        }

        /// <summary>
        /// Post on success should return "Created" 
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody]Camp model)
        {
            try
            {
                _logger.LogInformation($"[Post] Create a new item {model.Name}");
                _repo.Add(model);
                if (await _repo.SaveAllAsync())
                {
                    string uri = Url.Link("MyIndexGet", new {index = model.Id, speaker = true});
                    return Created(uri, model);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return BadRequest();

        }

        [HttpPatch("{index}")]
        [HttpPut("{index}")]
        public async Task<IActionResult> Put(int index, [FromBody] Camp model)
        {
            Camp camp = _repo.GetCamp(index);
            if (null == camp)
            {
                return NotFound(index);
            }

            // Doing update
            camp.Name = model.Name ?? camp.Name;
            camp.Description = model.Description ?? camp.Description;
            camp.Location = model.Location ?? camp.Location;

            try
            {
                if (await _repo.SaveAllAsync())
                {
                    return Ok(camp);
                }
            }
            catch
            {
                
            }

            return BadRequest("Update to camp failed.");
        }


        [HttpDelete("{index}")]
        public async Task<IActionResult> Delete(int index)
        {
            try
            {
                var camp = _repo.GetCamp(index);
                if (null == camp)
                {
                    return NotFound();
                }

                _repo.Delete(camp);
                if (await _repo.SaveAllAsync())
                {
                    return Ok();
                }
            }
            catch
            {
                
            }

            return BadRequest();

        }
    }
}