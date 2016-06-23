using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using SupportApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SupportApp.ViewModels.Connection;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private IEventService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventController(IEventService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }



        #region -----------------GET ALL EVENTS ---------------

        // GET: api/event/search/"seattle"
        [HttpGet]
        [Route("search/{city}")]
        public IActionResult GetSearch(string city)
        {
            List<Event> events = new List<Event>();
            events = _service.GetActiveEventsByCity(city);

            return Ok(events);
        }

        // GET: api/event/searchactivehistoryevents/"seattle"
        [HttpGet]
        [Route("searchactivehistoryevents/{city}")]
        public IActionResult GetSearchActiveHistoryEvents(string city)
        {
            List<Event> events = new List<Event>();
            events = _service.GetActiveHistoryEventsByCity(city);

            return Ok(events);
        }

        // GET: api/event/searchmyactiveevents/"seattle"
        [HttpGet]
        [Route("searchmyactiveevents/{city}")]
        [Authorize]
        public IActionResult GetSearchMyActiveEvents(string city)
        {
            List<Event> events = new List<Event>();
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            events = _service.GetMyCurrentEventsByCity(userId, hasClaim, city);

            return Ok(events);
        }

        // GET: api/event/searchmyactivehistoryevents/"seattle"
        [HttpGet]
        [Route("searchmyactivehistoryevents/{city}")]
        [Authorize]
        public IActionResult GetSearchMyActiveHistoryEvents(string city)
        {
            List<Event> events = new List<Event>();
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            events = _service.GetMyHistoryEventsByCity(userId, hasClaim, city);

            return Ok(events);
        }

        // GET: api/event/searchallevents/"seattle"
        [HttpGet]
        [Route("searchallevents/{city}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetSearchAllEvents(string city)
        {
            List<Event> events = new List<Event>();
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            events = _service.GetAdminEventsByCity(userId, hasClaim, city);

            return Ok(events);
        }

        // GET: api/event/searchallhistoryevents/"seattle"
        [HttpGet]
        [Route("searchallhistoryevents/{city}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetSearchAllHistoryEvents(string city)
        {
            List<Event> events = new List<Event>();
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            events = _service.GetAdminHistoryEventsByCity(userId, hasClaim, city);

            return Ok(events);
        }

        //when user is not login, comes here
        //if user is login, also comes here
        // GET: api/event/getactiveevents
        [HttpGet]
        [Route("getactiveevents")]
        public IActionResult GetActiveEvents()
        {
            var events = _service.GetActiveEvents();
            return Ok(events);
        }

        [HttpGet]
        [Route("gethistoryevents")]
        public IActionResult GetHistoryEvents()
        {
            var events = _service.GetHistoryEvents();
            return Ok(events);
        }

        // GET: api/event/getuserevents
        [HttpGet]
        [Route("getuserevents")]
        [Authorize]
        public IActionResult GetUserEvents()
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var events = _service.GetUserEvents(userId, hasClaim);
            return Ok(events);
        }

        // GET: api/event/getuserhistoryevents
        [HttpGet]
        [Route("getuserhistoryevents")]
        [Authorize]
        public IActionResult GetUserHistoryEvents()
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var events = _service.GetUserHistoryEvents(userId, hasClaim);
            return Ok(events);
        }

        // GET: api/event/getallevents
        [HttpGet]
        [Route("getallevents")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllEvents()
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var events = _service.GetAllEvents(hasClaim, userId);
            return Ok(events);
        }

        // GET: api/event/getallevents
        [HttpGet]
        [Route("getallhistoryevents")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAllHistoryEvents()
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var events = _service.GetAllHistoryEvents(hasClaim, userId);
            return Ok(events);
        }

        #endregion

        #region -----------------GET SINGLE EVENT ---------------
        // GET api/event/geteventdetails/5
        [HttpGet]
        [Route("geteventdetails/{id}")]
        public IActionResult GetEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            var singleEvent = _service.GetEventDetails(userId, id);
            return Ok(singleEvent);
        }

        // GET api/event/getusereventdetails/5
        [HttpGet]
        [Route("getusereventdetails/{id}")]
        [Authorize]
        public IActionResult GetUserEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var singleEvent = _service.GetUserEventDetails(userId, hasClaim, id);
            return Ok(singleEvent);
        }

        // GET api/event/getadmineventdetails/5
        [HttpGet]
        [Route("getadmineventdetails/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAdminEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var singleEvent = _service.GetUserEventDetails(userId, hasClaim, id);
            return Ok(singleEvent);
        }
        #endregion

        #region------------POST, PUT, and DELETE--------------------

        // POST api/event
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]EventLocationViewModel addedEvent)
        {
            if (addedEvent.EventStartDate == new System.DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("EventStartDate", "Event Start Date is required");
            }
            if (addedEvent.EventEndDate == new System.DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("EventEndDate", "Event End Date is required");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            _service.SaveEvent(userId, hasClaim, addedEvent);
            return Ok();
        }

        // DELETE api/event/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            _service.DeleteEvent(userId, hasClaim, id);

            return Ok();
        }

        // PUT api/event/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]int voteType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            //voteType is thumbs up or down 1 = up, 0 = down
            var sglEvent = _service.UpdateVotes(userId, id, voteType);
            return Ok(sglEvent);
        }

        #endregion
    }
}
