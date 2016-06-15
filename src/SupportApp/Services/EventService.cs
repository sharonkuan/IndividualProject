using Microsoft.EntityFrameworkCore;
using SupportApp.Models;
using SupportApp.Repositories;
using SupportApp.ViewModels.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Services
{
    public class EventService : IEventService
    {
        private IGenericRepository _repo;

        public EventService(IGenericRepository repo)
        {
            this._repo = repo;
        }

        public List<EventsViewModel> GetEvents()
        {
            var events = _repo.Query<Event>().Select(e => new EventsViewModel
            {
                Id = e.Id,
                EventType = e.EventType,
                EventTitle = e.EventTitle,
                Details = e.Details,
                EventStartDate = e.EventStartDate,
                EventEndDate = e.EventEndDate,
                DateCreated = e.DateCreated,
                IsActive = e.IsActive,
                IsComplete = e.IsComplete,
                IsPrivate = e.IsPrivate,
                IsVolunteerRequired = e.IsVolunteerRequired,
                PreferredNumberOfExpectedVolunteer = e.PreferredNumberOfExpectedVolunteer,
                UpVote = e.UpVote,
                DownVote = e.DownVote,
                Views = e.Views,
                Volunteers = e.EventUsers.Select(eu => eu.Member).ToList(),
                Comments = e.Comments.Select(c => c).ToList(),
                Locations = e.Locations.Select(el => el).ToList()
            }).OrderBy(e => e.EventStartDate).ToList();

            return events;
        }

        public EventsViewModel GetEvent(int id)
        {
            var sglEvent = _repo.Query<Event>().FirstOrDefault(e => e.Id == id);

            var volunteers = _repo.Query<EventUser>().Where(eu => eu.EventId == id).Select(e => e.Member).ToList();
            var sEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            sglEvent.Views++;
            _repo.SaveChanges();

            var singleEvent = new EventsViewModel
            {
                Id = sglEvent.Id,
                EventType = sglEvent.EventType,
                EventTitle = sglEvent.EventTitle,
                Details = sglEvent.Details,
                EventStartDate = sglEvent.EventStartDate,
                EventEndDate = sglEvent.EventEndDate,
                DateCreated = sglEvent.DateCreated,
                IsActive = sglEvent.IsActive,
                IsComplete = sglEvent.IsComplete,
                IsPrivate = sglEvent.IsPrivate,
                IsVolunteerRequired = sglEvent.IsVolunteerRequired,
                PreferredNumberOfExpectedVolunteer = sglEvent.PreferredNumberOfExpectedVolunteer,
                UpVote = sglEvent.UpVote,
                DownVote = sglEvent.DownVote,
                Views = sglEvent.Views,
                Volunteers = volunteers,
                Comments = sEvent.Comments,
                Locations = sEvent.Locations
            };

            return singleEvent;
        }

        //get events by city
        public List<Event> GetEventByCity(string city)
        {
            List<Event> events = new List<Event>();
            var allEvents = _repo.Query<Event>().Include(e=>e.Locations).ToList();

            foreach (var sglEvent in allEvents)
            {
                foreach (var item in sglEvent.Locations)
                {
                    if (item.City == city)
                    {
                        events.Add(sglEvent);
                    }
                } 
            }
            return events;
        }


        public Event SaveEvent(Event addedEvent)
        {
            if (addedEvent.Id == 0)
            {
                addedEvent.DateCreated = DateTime.UtcNow;
                _repo.Add<Event>(addedEvent);
            }
            else
            {
                var eventToEdit = _repo.Query<Event>().FirstOrDefault(e => e.Id == addedEvent.Id);
                eventToEdit.EventType = addedEvent.EventType;
                eventToEdit.EventTitle = addedEvent.EventTitle;
                eventToEdit.Details = addedEvent.Details;
                eventToEdit.EventStartDate = addedEvent.EventStartDate;
                addedEvent.EventEndDate = addedEvent.EventEndDate;
                eventToEdit.DateCreated = DateTime.UtcNow;
                eventToEdit.IsActive = eventToEdit.IsActive;
                addedEvent.IsComplete = addedEvent.IsComplete;
                eventToEdit.IsPrivate = addedEvent.IsPrivate;
                eventToEdit.IsVolunteerRequired = addedEvent.IsVolunteerRequired; eventToEdit.PreferredNumberOfExpectedVolunteer = addedEvent.PreferredNumberOfExpectedVolunteer;

                _repo.SaveChanges();
            }
            return addedEvent;
        }

        public void DeleteEvent(int id)
        {
            var sglEvent = _repo.Query<Event>().FirstOrDefault(e => e.Id == id);

            var volunteers = _repo.Query<EventUser>().Where(eu => eu.EventId == id).Select(e => e.Member).ToList();

            var singleEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();


            //Delete the related comments
            foreach (var comment in singleEvent.Comments)
            {
                var eventComment = _repo.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);
                _repo.Delete<Comment>(eventComment);
            }

            //Delete the related locations
            foreach (var location in singleEvent.Locations)
            {
                var eventLocation = _repo.Query<Location>().FirstOrDefault(c => c.Id == location.Id);
                _repo.Delete<Location>(eventLocation);
            }

            //delete the event
            _repo.Delete<Event>(singleEvent);

            ////delete the join table record
            //    var joinTableEventRecord = _repo.Query<EventUser>().Where(eu=>eu.EventId == id).ToList);
            //    _repo.Delete<EventUser>(volunteer);
        }

        public Event UpdateVotes(int id, int voteType)
        {
            //voteType is thumbs up or down 1 = up, 0 = down
            var singleEvent = _repo.Query<Event>().Where(e => e.Id == id).FirstOrDefault();
            if (voteType == 1)
            {
                singleEvent.UpVote++;
            }
            else if (voteType == 0)
            {
                singleEvent.DownVote++;
            }

            _repo.Update(singleEvent);
            return singleEvent;
        }

    }
}
