using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Models;
using SupportApp.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventLocationController : Controller
    {
        private ApplicationDbContext _db;

        public EventLocationController(ApplicationDbContext db)
        {
            this._db = db;
        }

        //allows adding one to many location for an event
        [HttpPost("{id}")]
        public IActionResult Post(int id, [FromBody]Location location)
        {
            //find the blog and bring back all the related comments
            var selectedEvent = _db.Events.Where(e => e.Id == id).Include(e => e.Locations).FirstOrDefault();
            //add the comment to the single blog and 
            selectedEvent.Locations.Add(location);
            //then save to the database
            _db.SaveChanges();
            return Ok();
        }
    }
}
