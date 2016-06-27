using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportApp.Data;
using SupportApp.Models;
using SupportApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Services
{
    public class EventLocationService : IEventLocationService
    {
        private IGenericRepository _repo;

        public EventLocationService(IGenericRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._repo = repo;
        }

        public Event SaveEditedUserEvent(string userId, bool hasClaim, int eventId, Event eventData)
        {
            var eventToEdit = _repo.Query<Event>().Where(e => e.Id == eventId).Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault();

            eventToEdit.EventType = eventData.EventType;
            eventToEdit.EventTitle = eventData.EventTitle;
            eventToEdit.Details = eventData.Details;
            eventToEdit.EventStartDate = eventData.EventStartDate.ToUniversalTime();
            eventToEdit.EventEndDate = eventData.EventEndDate.ToUniversalTime();
            eventToEdit.IsComplete = eventData.IsComplete;
            eventToEdit.IsPrivate = eventData.IsPrivate;
            eventToEdit.IsVolunteerRequired = eventData.IsVolunteerRequired;
            eventToEdit.PreferredNumberOfExpectedVolunteer = eventData.PreferredNumberOfExpectedVolunteer;

            eventToEdit.NumberOfVolunteerRegistered = eventToEdit.NumberOfVolunteerRegistered;
            eventToEdit.ApprovedByAdmin = eventToEdit.ApprovedByAdmin;
            eventToEdit.UpVote = eventToEdit.UpVote;
            eventToEdit.DownVote = eventToEdit.DownVote;
            eventToEdit.Views = eventToEdit.Views;
            eventToEdit.ApplicationUserId = eventToEdit.ApplicationUserId;
            eventToEdit.DateCreated = eventToEdit.DateCreated;
            eventToEdit.IsActive = eventToEdit.IsActive;

            _repo.SaveChanges();

            eventToEdit = EventMarkUp(eventToEdit);
            return eventToEdit;
        }

        public Event SaveAddedLocation(int eventId, string userId, Location location)
        {
            var selectedEvent = _repo.Query<Event>().Where(e => e.Id == eventId).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            location.DateCreated = DateTime.UtcNow;
            location.CreatedBy = userId;
            location.IsActive = true;

            selectedEvent.Locations.Add(location);
            _repo.SaveChanges();
            selectedEvent = EventMarkUp(selectedEvent);
            return selectedEvent;
        }

        public Location GetLocation(int locationId)
        {
            var selectedLocation = _repo.Query<Location>().Where(l => l.Id == locationId).FirstOrDefault();
            return selectedLocation;
        }

        public Location SaveLocation(int locationId, string userId, Location location)
        {
            var selectedEventLocation = _repo.Query<Location>().Where(el => el.Id == locationId).FirstOrDefault();
            selectedEventLocation.NameOfLocation = location.NameOfLocation;
            selectedEventLocation.Address = location.Address;
            selectedEventLocation.City = location.City;
            selectedEventLocation.State = location.State;
            selectedEventLocation.Zip = location.Zip;
            selectedEventLocation.IsActive = true;
            selectedEventLocation.DateCreated = selectedEventLocation.DateCreated;
            selectedEventLocation.CreatedBy = userId;
            _repo.SaveChanges();

            return selectedEventLocation;
        }

        #region Markup methods
        public Event EventMarkUp(Event sglEvent)
        {
            var userId = sglEvent.ApplicationUserId;
            var userFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.FirstName).FirstOrDefault();
            var userLastName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.LastName).FirstOrDefault();
            sglEvent.ApplicationUserId = userFirstName + " " + userLastName;

            foreach (var comment in sglEvent.Comments)
            {
                var commentWriterId = comment.ApplicationUserId;
                var commentWriterFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == commentWriterId).Select(au => au.FirstName).FirstOrDefault();
                comment.ApplicationUserId = commentWriterFirstName;
                comment.DateCreated = comment.DateCreated.ToLocalTime();
            }

            return sglEvent;
        }

        public Event convertDatesFromUtcToLocalTime(Event sglEvent)
        {
            sglEvent.EventStartDate = sglEvent.EventStartDate.ToLocalTime();
            sglEvent.EventEndDate = sglEvent.EventEndDate.ToLocalTime();
            sglEvent.DateCreated = sglEvent.DateCreated.ToLocalTime();

            return sglEvent;
        }

        public Event DisplayUserFirstName(Event sglEvent)
        {
            var userId = sglEvent.ApplicationUserId;
            var userFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.FirstName).FirstOrDefault();
            sglEvent.ApplicationUserId = userFirstName;

            foreach (var comment in sglEvent.Comments)
            {
                var commentWriterId = comment.ApplicationUserId;
                var commentWriterFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == commentWriterId).Select(au => au.FirstName).FirstOrDefault();
                comment.ApplicationUserId = commentWriterFirstName;
            }

            return sglEvent;
        }
        #endregion
    }
}