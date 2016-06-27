using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using SupportApp.Data;
using Microsoft.EntityFrameworkCore;
using SupportApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventLocationController : Controller
    {
        private IEventLocationService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventLocationController(IEventLocationService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var locationToEdit = _service.GetLocation(id);

            return Ok(locationToEdit);
        }

        //only Admin is allowed to add additional event locations to an existing event
        [HttpPost]
        [Route("saveaddedeventlocation/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult SaveAddedEventLocation(int id, [FromBody]Location location)
        {
            var addedEventAddress = new Event();

            if (location == null || String.IsNullOrWhiteSpace(location.ToString()))
            {
                ModelState.AddModelError("NameOfLocation", "Location name is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var userId = _userManager.GetUserId(User);

            if (userId != null && location.Id == 0)
            {
                addedEventAddress = _service.SaveAddedLocation(id, userId, location);
            }

            return Ok(addedEventAddress);
        }

        // POST api/eventLocation/saveusereventedit/:id
        [HttpPost]
        [Route("saveusereventedit/{id}")]
        [Authorize]
        public IActionResult SaveUserEventEdit(int id, [FromBody]Event eventToEdit)
        {
            if (eventToEdit.EventStartDate == new System.DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("EventStartDate", "Event Start Date is required");
            }
            if (eventToEdit.EventEndDate == new System.DateTime(0001, 01, 01))
            {
                ModelState.AddModelError("EventEndDate", "Event End Date is required");
            }
            if (eventToEdit.EventEndDate < eventToEdit.EventStartDate)
            {
                ModelState.AddModelError("EventEndDate", "Event End Date has to be later than the start date");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = _userManager.GetUserId(User);
            bool hasClaim = User.HasClaim("IsAdmin", "true");
            eventToEdit = _service.SaveEditedUserEvent(userId, hasClaim, id, eventToEdit);
            return Ok(eventToEdit);
        }

        [HttpPost]
        [Route("savelocation/{id}")]
        [Authorize]
        public IActionResult SaveLocation(int id, [FromBody] Location location)
        {
            if (location == null || String.IsNullOrWhiteSpace(location.ToString()))
            {
                ModelState.AddModelError("NameOfLocation", "Location name is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var userId = _userManager.GetUserId(User);
         
            location = _service.SaveLocation(id, userId, location);
            return Ok(location);
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var selectedEventLocations = _db.Events.Where(e => e.Id == id).Include(e => e.Locations).Select(e => e.Locations).ToList();

        //    if (id == 0 || selectedEventLocations == null)
        //    {
        //        return BadRequest();
        //    }

        //    foreach (Location location in selectedEventLocations)
        //    {
        //        _db.Locations.Remove(location);
        //    };

        //    _db.SaveChanges();
        //    return Ok();
        //}
    }
}
