using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPITest.Data;
using WebAPITest.Data.Entities;

namespace WebAPITest.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : Controller
    {
        private ICampRepository _repo;
        private ILogger<CampsController> _logger;

        public CampsController(ICampRepository repo, ILogger<CampsController> logger)
        {
            _repo = repo;
            _logger = logger;

        }

        [HttpGet("get1")]
        public IActionResult Get1()
        {
            var camp = _repo.GetAllCamps();
            var addr = _repo.GetHashCode();

            // Anonmous Type
            // Ok -> 200
            return Ok(camp);
        }

        [HttpGet("[action]")]
        public IActionResult Get2()
        {
            var camp = _repo.GetAllCamps();
            var addr = _repo.GetHashCode();

            // Anonmous Type
            // Ok -> 200
            return Ok(camp);
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
                return Ok(camp);
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
    }
}