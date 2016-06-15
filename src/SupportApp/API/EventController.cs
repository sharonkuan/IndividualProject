using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Data;
using SupportApp.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using SupportApp.Models;
using SupportApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private IEventService _service;
        private ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(IEventService service, UserManager<ApplicationUser> userManager )
        {
            _service = service;
            _userManager = userManager;
        }

        // GET: api/events/search
        [HttpGet]
        [Authorize]
        public IActionResult Search()
        {
            var userId = _userManager.GetUserId(this.User);
            
            var events = _service.GetEvents();

            return Ok(events);
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var events = _service.GetEvents(); 

            return Ok(events);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var singleEvent = _service.GetEvent(id);
            return Ok(singleEvent);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Event addedEvent)
        {
            _service.SaveEvent(addedEvent);
            return Ok(addedEvent);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]int voteType)
        {
            //voteType is thumbs up or down 1 = up, 0 = down
            var sglEvent = _service.UpdateVotes(id, voteType);
            return Ok(sglEvent);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
