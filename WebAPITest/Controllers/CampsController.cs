using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Data;
using WebAPITest.Data.Entities;

namespace WebAPITest.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : Controller
    {
        private ICampRepository _repo;

        public CampsController(ICampRepository repo)
        {
            _repo = repo;
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

        [HttpGet("{index}")]
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
    }
}