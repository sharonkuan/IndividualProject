using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using SupportApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
        /// <summary>
        /// This returns all events 
        /// </summary>
        /// <returns></returns>
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

        // GET: api/event/getuserevents
        [HttpGet]
        [Route("getuserevents")]
        [Authorize]
        public IActionResult GetUserEvents()
        {
            var userId = _userManager.GetUserId(User);
            var events = _service.GetUserEvents(userId);
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
        #endregion

        #region -----------------GET SINGLE EVENT ---------------

        // GET api/event/getuserevent/5
        [HttpGet]
        [Route("getuserevent/{id}")]
        [Authorize]
        public IActionResult GetUserEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            var singleEvent = _service.GetUserEventDetails(userId, hasClaim, id);
            return Ok(singleEvent);
        }

        // GET: api/event/search/"seattle"
        [HttpGet]
        [Route("search/{city}")]
        public IActionResult GetSearch(string city)
        {
            var events = _service.GetActiveEventsByCity(city);
            return Ok(events);
        }

        // GET api/event/geteventdetails/5
        [HttpGet]
        [Route("geteventdetails/{id}")]
        public IActionResult GetEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            var singleEvent = _service.GetEventDetails(userId, id);
            return Ok(singleEvent);
        }

        //[HttpGet]
        //[Route("gethistoryevent/{id}")]
        //public IActionResult GetHistoryEventDetails(int id)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var singleEvent = _service.GetHistoryEventDetails(userId, id);
        //    return Ok(singleEvent);
        //}
        #endregion

        #region------------POST, PUT, and DELETE--------------------

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Event addedEvent)
        {
            var userId = _userManager.GetUserId(User);
            _service.SaveEvent(userId, addedEvent);
            return Ok(addedEvent);
        }

        // PUT api/event/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]int voteType)
        {
            var userId = _userManager.GetUserId(User);
            //voteType is thumbs up or down 1 = up, 0 = down
            var sglEvent = _service.UpdateVotes(userId, id, voteType);
            return Ok(sglEvent);
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
        #endregion
    }
}
