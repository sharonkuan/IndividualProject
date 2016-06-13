using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Data;
using SupportApp.ViewModels.Event;
using Microsoft.EntityFrameworkCore;
using SupportApp.Models;

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
            ////    get database query to grab the event
            //    var sglEvent = _db.Events.FirstOrDefault(e => e.Id == id);
            //sglEvent.Views++;
            //    _db.SaveChanges();

            //    var singleEvent = new EventsViewModel
            //    {
            //        Id = sglEvent.Id,
            //        EventType = sglEvent.EventType,
            //        EventTitle = sglEvent.EventTitle,
            //        Details = sglEvent.Details,
            //        EventDate = sglEvent.EventDate,
            //        StartHour = sglEvent.StartHour,
            //        StartMinutes = sglEvent.StartMinutes,
            //        StartTimeIsAmPm = sglEvent.StartTimeIsAmPm,
            //        EndHour = sglEvent.EndHour,
            //        EndMinutes = sglEvent.EndMinutes,
            //        EndTimeIsAmPm = sglEvent.EndTimeIsAmPm,
            //        DateCreated = sglEvent.DateCreated,
            //        IsPrivate = sglEvent.IsPrivate,
            //        IsVolunteerRequired = sglEvent.IsVolunteerRequired,
            //        PreferredNumberOfExpectedVolunteer = sglEvent.PreferredNumberOfExpectedVolunteer,
            //        UpVote = sglEvent.UpVote,
            //        DownVote = sglEvent.DownVote,
            //        Views = sglEvent.Views,
            //        Volunteers = sglEvent.EventUsers.Select(eu => eu.Member).ToList(),
            //        Comments = sglEvent.Comments.Select(c => c).ToList(),
            //        Locations = sglEvent.Locations.Select(el => el).ToList()
            //    };
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

            return Ok(singleEvent);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Event addedEvent)
        {
            if (addedEvent.Id == 0)
            {
                addedEvent.DateCreated = DateTime.UtcNow;
                _db.Events.Add(addedEvent);
                _db.SaveChanges();
            }
            else
            {
                var eventToEdit = _db.Events.FirstOrDefault(e => e.Id == addedEvent.Id);
                eventToEdit.EventType = addedEvent.EventType;
                eventToEdit.EventTitle = addedEvent.EventTitle;
                eventToEdit.Details = addedEvent.Details;
                eventToEdit.EventDate = addedEvent.EventDate;
                eventToEdit.StartHour = addedEvent.StartHour;
                eventToEdit.StartMinutes = addedEvent.StartMinutes;
                eventToEdit.StartTimeIsAmPm = addedEvent.StartTimeIsAmPm;
                eventToEdit.EndHour = addedEvent.EndHour;
                eventToEdit.EndMinutes = addedEvent.EndMinutes;
                eventToEdit.EndTimeIsAmPm = addedEvent.EndTimeIsAmPm;
                eventToEdit.DateCreated = DateTime.UtcNow;
                eventToEdit.IsPrivate = addedEvent.IsPrivate;
                eventToEdit.IsVolunteerRequired = addedEvent.IsVolunteerRequired;
                eventToEdit.PreferredNumberOfExpectedVolunteer = addedEvent.PreferredNumberOfExpectedVolunteer;

                _db.SaveChanges();
            }

            return Ok(addedEvent);
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
            var singleEvent = _db.Events.Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();
            //var singleEvent = _db.Events.Where(e => e.Id == id).Include(e => new
            //{
            //    Comments = e.Comments.Select(c => c).ToList(),
            //    Locations = e.Locations.Select(el => el).ToList()
            //}).FirstOrDefault();

            if (id == 0 || singleEvent == null)
            {
                return BadRequest();
            }

            //foreach (var item in singleEvent.Comments)
            //{
            //    _db.Comments.Remove(item);
            //}

            _db.Events.Remove(singleEvent);
            _db.SaveChanges();
            return Ok();
        }
    }
}
