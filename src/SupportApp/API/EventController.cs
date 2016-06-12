using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Data;
using SupportApp.ViewModels.Event;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        private ApplicationDbContext _db;

        public EventController(ApplicationDbContext db)
        {
            this._db = db;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            //var movies = _db.Movies.Select(m => new MoviesViewModel
            //{
            //    Title = m.Title,
            //    Actors = m.MovieActors.Select(ma => ma.Actor).ToList()
            //});
            var events = _db.Events.Select(e => new EventsViewModel
            {
                Id = e.Id,
                EventType = e.EventType,
                EventTitle = e.EventTitle,
                Details = e.Details,
                EventDate = e.EventDate,
                StartHour = e.StartHour,
                StartMinutes = e.StartMinutes,
                StartTimeIsAmPm = e.StartTimeIsAmPm,
                EndHour = e.EndHour,
                EndMinutes = e.EndMinutes,
                EndTimeIsAmPm = e.EndTimeIsAmPm,
                DateCreated = e.DateCreated,
                IsCompleted = e.IsCompleted,
                IsPrivate = e.IsPrivate,
                IsVolunteerRequired = e.IsVolunteerRequired,
                PreferredNumberOfExpectedVolunteer = e.PreferredNumberOfExpectedVolunteer,
                UpVote = e.UpVote,
                DownVote = e.DownVote,
                Views = e.Views,
                Volunteers = e.EventUsers.Select(eu => eu.Member).ToList(),
                Comments = e.Comments.Select(c => c).ToList(),
                Locations = e.Locations.Select(el => el).ToList()
            }).OrderBy(e => e.EventDate).ToList();

            return Ok(events);
        }



        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var singleEvent = _db.Events.Where(e => e.Id == id).Select(e => new EventsViewModel
            {
                Id = e.Id,
                EventType = e.EventType,
                EventTitle = e.EventTitle,
                Details = e.Details,
                EventDate = e.EventDate,
                StartHour = e.StartHour,
                StartMinutes = e.StartMinutes,
                StartTimeIsAmPm = e.StartTimeIsAmPm,
                EndHour = e.EndHour,
                EndMinutes = e.EndMinutes,
                EndTimeIsAmPm = e.EndTimeIsAmPm,
                DateCreated = e.DateCreated,
                IsCompleted = e.IsCompleted,
                IsPrivate = e.IsPrivate,
                IsVolunteerRequired = e.IsVolunteerRequired,
                PreferredNumberOfExpectedVolunteer = e.PreferredNumberOfExpectedVolunteer,
                UpVote = e.UpVote,
                DownVote = e.DownVote,
                Views = e.Views,
                Volunteers = e.EventUsers.Select(eu => eu.Member).ToList(),
                Comments = e.Comments.Select(c => c).ToList(),
                Locations = e.Locations.Select(el => el).ToList()
            }).FirstOrDefault();

            singleEvent.Views++;

            _db.SaveChanges();

            return Ok(singleEvent);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]int voteType)
        {
            //voteType is thumbs up or down 1 = up, 0 = down
            var singleEvent = _db.Events.Where(e => e.Id == id).Include(e => e.Comments).FirstOrDefault();
            if (voteType == 1)
            {
                singleEvent.UpVote++;
            }
            else if (voteType == 0)
            {
                singleEvent.DownVote++;
            }
            _db.SaveChanges();
            return Ok(singleEvent);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var singleEvent = _db.Events.Where(e => e.Id == id).Include(e => e.Comments).Include(e=>e.Locations).FirstOrDefault();
            _db.Events.Remove(singleEvent);
            _db.SaveChanges();
            return Ok();
        }
    }
}
