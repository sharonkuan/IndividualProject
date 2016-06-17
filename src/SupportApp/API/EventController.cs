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

        //// GET: api/events/search
        //[HttpGet("Search")]
        //[Authorize]
        //public IActionResult Search()
        //{
        //    var events = _service.GetAllEvents();

        //    return Ok(events);
        //}

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

        // GET api/event/getactiveevent/5
        [HttpGet]
        [Route("getactiveevent/{id}")]
        public IActionResult GetActiveEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            //var singleEvent = _service.GetActiveEventDetails(id);
            var singleEvent = _service.GetActiveEventDetails(userId, id);
            return Ok(singleEvent);
        }

        [HttpGet]
        [Route("gethistoryevent/{id}")]
        public IActionResult GetHistoryEventDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            //var singleEvent = _service.GetActiveEventDetails(id);
            var singleEvent = _service.GetHistoryEventDetails(userId, id);
            return Ok(singleEvent);
        }
        #endregion

        #region------------POST, PUT, and DELETE--------------------

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
        #endregion
    }
}
