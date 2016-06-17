using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportApp.Data;
using SupportApp.Models;
using SupportApp.Repositories;
using SupportApp.ViewModels.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SupportApp.Services
{
    public class EventService : IEventService
    {
        private IGenericRepository _repo;

        public EventService(IGenericRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._repo = repo;
        }

        /// <summary>
        /// anyone can see the events when it's not login
        /// </summary>
        /// <returns></returns>
        public EventsViewModel GetActiveEvents()
        {
            var vm = new EventsViewModel();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).OrderBy(e => e.EventStartDate).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).ToList();
            vm.CanEdit = false;

            return vm;
        }

        /// <summary>
        /// anyone can see the events when it's not login
        /// </summary>
        /// <returns></returns>
        public EventsViewModel GetHistoryEvents()
        {
            var vm = new EventsViewModel();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).OrderBy(e => e.EventStartDate).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.IsComplete == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).ToList();
            vm.CanEdit = false;

            return vm;
        }

        /// <summary>
        /// returns a list of events that are associated with the current logged in user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventsViewModel GetUserEvents(string id)
        {
            var vm = new EventsViewModel();

            vm.Events = _repo.Query<Event>().Where(e => e.ApplicationUserId == id).Include(e => e.Comments).Include(e => e.Locations).OrderBy(e => e.EventStartDate).ToList();

            vm.CanEdit = true;

            return vm;
        }

        /// <summary>
        /// returning one vm of list for Admin and if not admin, return only the active events to general viewers
        /// </summary>
        /// <returns></returns>
        public EventsViewModel GetAllEvents(bool hasClaim, string id)
        {

            var vm = new EventsViewModel();

            if (hasClaim)
            {
                vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).OrderBy(e => e.EventStartDate).ToList();
                vm.CanEdit = true;  //no need to save as its contain in the view model
            }
            else
            {
                ////These code is not needed for now
                ////save for new claims for members that are volunteers to select which one they want to volunteer
                //var user = _repo.Query<ApplicationUser>().Where(u => u.Id == id).Include(u => u.Events).FirstOrDefault();
                //vm.Events = user.Events.OrderBy(e => e.EventStartDate).ToList();
                //vm.CanEdit = false;
            }

            return vm;
        }



        /// <summary>
        /// get the single event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventDetailsViewModel GetUserEventDetails(string userId, bool hasClaim, int id)
        {
            var vm = new EventDetailsViewModel();
            var sglEvent = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);

            if (hasClaim)
            {
                vm.CanEdit = true;
            }
            else
            {
                //if the event is the user's event, then user can edit, else user can only vote
                if (sglEvent.ApplicationUserId == userId)
                {
                    vm.CanEdit = true;
                }
                else
                {
                    vm.CanEdit = false;
                }
            }

            vm.Event = sglEvent;
            sglEvent.Views++;
            _repo.SaveChanges();
            return vm;  //usereventdetails
        }

        public EventDetailsViewModel GetActiveEventDetails(string userId,int id)
        {
            var vm = new EventDetailsViewModel();
            var eventSelected = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);
           
            if (userId != null)
            {
                vm.CanEdit = true;
            }

            else
            {
                vm.CanEdit = false;
            }

            vm.Event = eventSelected;
            eventSelected.Views++;
            _repo.SaveChanges();
            return vm;  //activeeventdetails
        }

        public EventDetailsViewModel GetHistoryEventDetails(string userId, int id)
        {
            var vm = new EventDetailsViewModel();
            var eventSelected = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);

            if (userId != null)
            {
                vm.CanEdit = true;
            }
            else
            {

                vm.CanEdit = false;
            }

            vm.Event = eventSelected;
            eventSelected.Views++;
            _repo.SaveChanges();
            return vm;
        }

        //get events by city
        public List<Event> GetEventByCity(string city)
        {
            List<Event> events = new List<Event>();
            var allEvents = _repo.Query<Event>().Include(e => e.Locations).ToList();

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
