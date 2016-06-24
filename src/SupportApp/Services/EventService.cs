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
using Microsoft.AspNetCore.Authorization;

namespace SupportApp.Services
{
    public class EventService : IEventService
    {
        private IGenericRepository _repo;

        public EventService(IGenericRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            this._repo = repo;
        }

        public List<ApplicationUser> GetVolunteerInfo(int eventId)
        {
            List<ApplicationUser> eventList = _repo.Query<EventUser>().Where(eu => eu.EventId == eventId).Select(eu=> eu.Member).ToList();
            return eventList;
        }

        #region Home page Search by City - Active events and History events (isActive and isComplete only)
        //===============================   HOME PAGE  ================================================
        /// <summary>
        /// Home page - results by city selected by the viewer 
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public List<Event> GetActiveEventsByCity(string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = false;
            vm.HasClaim = false;

            filteredEventsList = FilterEventsByCity(vm, city);

            return filteredEventsList;
        }

        public List<Event> GetActiveHistoryEventsByCity(string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.IsComplete == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = false;
            vm.HasClaim = false;

            filteredEventsList = FilterEventsByCity(vm, city);

            return filteredEventsList;
        }

        #endregion

        #region My Event Page

        //kept the hasClaim - will try if can combine into one html to handle my event and admin event features
        public List<Event> GetMyCurrentEventsByCity(string userId, bool hasClaim, string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();

            vm.Events = _repo.Query<Event>().Where(e => e.ApplicationUserId == userId).Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;

            filteredEventsList = FilterEventsByCity(vm, city);
            return filteredEventsList;
        }

        public List<Event> GetMyHistoryEventsByCity(string userId, bool hasClaim, string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();

            vm.Events = _repo.Query<Event>().Where(e => e.ApplicationUserId == userId).Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            //vm.Events = vm.Events.Where(e => e.IsComplete == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;

            filteredEventsList = FilterEventsByCity(vm, city);
            return filteredEventsList;
        }
        #endregion

        #region Admin page

        public List<Event> GetAdminEventsByCity(string userId, bool hasClaim, string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();

            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;

            filteredEventsList = FilterEventsByCity(vm, city);
            return filteredEventsList;
        }

        public List<Event> GetAdminHistoryEventsByCity(string userId, bool hasClaim, string city)
        {
            var vm = new EventsViewModel();
            var filteredEventsList = new List<Event>();

            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;

            filteredEventsList = FilterEventsByCity(vm, city);
            return filteredEventsList;
        }

        #endregion

        /// <summary>
        /// Home page - anyone can see the events when it's not login
        /// </summary>
        /// <returns></returns>
        public EventsViewModel GetActiveEvents()
        {
            var vm = new EventsViewModel();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = false;
            vm.HasClaim = false;

            return vm;
        }

        /// <summary>
        /// Home paeg - anyone can see the events when it's not login
        /// </summary>
        /// <returns></returns>
        public EventsViewModel GetHistoryEvents()
        {
            var vm = new EventsViewModel();
            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.IsComplete == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = false;
            vm.HasClaim = false;

            return vm;
        }


        #region My Event and Admin Event - canEdit if userId match the event creator or is an admin, hasClaim based on user's role
        /// <summary>
        /// My Event page - returns a list of events that are associated with the current logged in user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hasClaim"></param>
        /// <returns></returns>
        public EventsViewModel GetUserEvents(string id, bool hasClaim)
        {
            var vm = new EventsViewModel();

            vm.Events = _repo.Query<Event>().Where(e => e.ApplicationUserId == id).Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;
            return vm;
        }

        public EventsViewModel GetUserHistoryEvents(string id, bool hasClaim)
        {
            var vm = new EventsViewModel();

            vm.Events = _repo.Query<Event>().Where(e => e.ApplicationUserId == id).Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.IsActive == true).ToList();
            //vm.Events = vm.Events.Where(e => e.IsComplete == true).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm.CanEdit = true;
            vm.HasClaim = hasClaim;
            return vm;
        }

        /// <summary>
        /// Admin page - returning one vm of list for Admin and if not admin, return only the active events to general viewers
        /// </summary>
        /// <param name="hasClaim"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventsViewModel GetAllEvents(bool hasClaim, string id)
        {
            var vm = new EventsViewModel();

            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate > DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            vm.CanEdit = true;  //no need to save as its contain in the view model
            vm.HasClaim = hasClaim;

            return vm;
        }

        public EventsViewModel GetAllHistoryEvents(bool hasClaim, string id)
        {
            var vm = new EventsViewModel();

            vm.Events = _repo.Query<Event>().Include(e => e.Comments).Include(e => e.Locations).ToList();
            vm.Events = vm.Events.Where(e => e.EventStartDate < DateTime.UtcNow).OrderBy(e => e.EventStartDate).ToList();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            vm.CanEdit = true;  //no need to save as its contain in the view model
            vm.HasClaim = hasClaim;

            return vm;
        }
        #endregion

        #region Event Details Page - Home, My Event and Admin Event
        /// <summary>
        /// Home apge Event details - when user is a member, they can add comments
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventDetailsViewModel GetEventDetails(string userId, int id)
        {
            var vm = new EventDetailsViewModel();
            var eventSelected = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);

            if (userId != null)
            {
                vm.CanEdit = true;
                vm.HasClaim = false;
            }

            else
            {
                vm.CanEdit = false;
                vm.HasClaim = false;
            }

            vm.Event = eventSelected;
            eventSelected.Views++;
            _repo.SaveChanges();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            return vm;  //activeeventdetails
        }

        /// <summary>
        /// My Event and Admin Event Details page - checks if this user can edit the single event, this can be used for CRUD
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hasClaim"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public EventDetailsViewModel GetUserEventDetails(string userId, bool hasClaim, int id)
        {
            var vm = new EventDetailsViewModel();
            var sglEvent = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);

            if (hasClaim)
            {
                vm.CanEdit = true;
                vm.HasClaim = hasClaim;
            }
            else
            {
                //if the event is the user's event, then user can edit, else user can only vote
                if (sglEvent.ApplicationUserId == userId)
                {
                    vm.CanEdit = true;
                    vm.HasClaim = hasClaim;
                }
                else
                {
                    vm.CanEdit = false;
                    vm.HasClaim = false;
                }
            }

            vm.Event = sglEvent;
            sglEvent.Views++;
            _repo.SaveChanges();
            vm = convertDatesFromUtcToLocalTime(vm);
            vm = DisplayUserName(vm);
            return vm;  //usereventdetails or for admin
        }
        #endregion

        //ToDo: If a member Deletes its event, an email is sent to the Admin
        //- Both Members can Add, Edit and Delete their own events (limited to one location per event), 
        //Admin has permission to CRUD all events and Add Additional Location to an Event (if requried).
        #region My Event and Admin Event - Create, Delete, and Update
        /// <summary>
        /// Both Member(My Event) and Admin - Saves an event with one location
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hasClaim"></param>
        /// <param name="addedEvent"></param>
        /// <returns></returns>
        public void SaveEvent(string userId, bool hasClaim, EventLocationViewModel addedEvent)
        {
            Event eventData;

            if (addedEvent.EventId == 0)
            {
                eventData = new Event
                {
                    EventTitle = addedEvent.EventTitle,
                    EventType = addedEvent.EventType,
                    Details = addedEvent.Details,
                    PreferredNumberOfExpectedVolunteer = addedEvent.PreferredNumberOfExpectedVolunteer,
                    DownVote = 0,
                    UpVote = 0,
                    EventStartDate = addedEvent.EventStartDate.ToUniversalTime(),
                    EventEndDate = addedEvent.EventEndDate.ToUniversalTime(),
                    IsComplete = addedEvent.IsComplete,
                    IsPrivate = addedEvent.IsPrivate,
                    IsVolunteerRequired = addedEvent.IsVolunteerRequired,
                    Views = 0,
                    ApplicationUserId = userId,
                    NumberOfVolunteerRegistered = 0,
                    ApprovedByAdmin = false,
                    DateCreated = DateTime.UtcNow,
                    IsActive = true
                };
                _repo.Add(eventData);
                var newEvent = _repo.Query<Event>().Where(e => e.Id == eventData.Id).Include(e => e.Locations).FirstOrDefault();
                //adds the location
                newEvent.Locations.Add(new Location
                {
                    NameOfLocation = addedEvent.NameOfLocation,
                    Address = addedEvent.Address,
                    City = addedEvent.City,
                    State = addedEvent.State,
                    Zip = addedEvent.Zip,
                    IsActive = true,
                    DateCreated = DateTime.UtcNow,
                    CreatedBy = userId
                });

                _repo.SaveChanges();
            }
            else
            {
                var eventToEdit = _repo.Query<Event>().Where(e => e.Id == addedEvent.EventId).Include(e => e.Locations).FirstOrDefault();

                eventToEdit.EventTitle = addedEvent.EventTitle;
                eventToEdit.EventStartDate = addedEvent.EventStartDate.ToUniversalTime();
                eventToEdit.EventEndDate = addedEvent.EventEndDate.ToUniversalTime();
                eventToEdit.IsComplete = addedEvent.IsComplete;
                eventToEdit.IsPrivate = addedEvent.IsPrivate;
                eventToEdit.IsVolunteerRequired = addedEvent.IsVolunteerRequired;
                eventToEdit.Views = eventToEdit.Views;
                eventToEdit.ApplicationUserId = eventToEdit.ApplicationUserId;
                eventToEdit.DateCreated = eventToEdit.DateCreated;
                eventToEdit.IsActive = true;
                eventToEdit.NumberOfVolunteerRegistered = eventToEdit.NumberOfVolunteerRegistered;
                eventToEdit.ApprovedByAdmin = eventToEdit.ApprovedByAdmin;

                //eventToEdit.Locations.Add(new Location
                //{
                //    NameOfLocation = addedEvent.LocationNameOfLocation,
                //    Address = addedEvent.LocationAddress,
                //    City = addedEvent.LocatinoCity,
                //    State = addedEvent.LocationState,
                //    Zip = addedEvent.LocationZip,
                //    IsActive = true,
                //    DateCreated = DateTime.UtcNow,
                //});

                _repo.SaveChanges();
            }
        }

        /// <summary>
        /// Checks user credential before doing a soft delete on the events and its location and comments
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="hasClaim"></param>
        /// <param name="id"></param>
        public void DeleteEvent(string userId, bool hasClaim, int id)
        {
            //TODO
            var volunteers = _repo.Query<EventUser>().Where(eu => eu.EventId == id).Select(e => e.Member).ToList();

            var singleEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            if (hasClaim)
            {
                SoftDeleteEvet(singleEvent);
            }
            else if (singleEvent.ApplicationUserId == userId)
            {
                SoftDeleteEvet(singleEvent);
            }
        }
        #endregion

        public List<Event> FilterEventsByCity(EventsViewModel vm, string city)
        {
            var filteredEventsList = new List<Event>();

            foreach (var singleEvent in vm.Events)
            {
                foreach (var location in singleEvent.Locations)
                {
                    if (location.City == city)
                    {
                        filteredEventsList.Add(singleEvent);
                        break;
                    }
                }
            }
            return filteredEventsList;
        }

        public Event UpdateVotes(string userId, int id, int voteType)
        {
            //voteType is thumbs up or down 1 = up, 0 = down
            var singleEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            if (userId != null)
            {
                if (voteType == 1)
                {
                    singleEvent.UpVote++;
                }
                else if (voteType == 0)
                {
                    singleEvent.DownVote++;
                }
            }
            _repo.Update(singleEvent);
            singleEvent = EventMarkUp(singleEvent);
            return singleEvent;
        }

        /// <summary>
        /// function to do the soft delete of an event (event, locations, and comments models)
        /// </summary>
        /// <param name="singleEvent"></param>
        public void SoftDeleteEvet(Event singleEvent)
        {
            //soft Delete the related comments
            foreach (var comment in singleEvent.Comments)
            {
                var eventComment = _repo.Query<Comment>().FirstOrDefault(c => c.Id == comment.Id);
                eventComment.IsActive = false;
                _repo.Update<Comment>(eventComment);
            }

            //soft Delete the related locations
            foreach (var location in singleEvent.Locations)
            {
                var eventLocation = _repo.Query<Location>().FirstOrDefault(c => c.Id == location.Id);
                eventLocation.IsActive = false;
                _repo.Update<Location>(eventLocation);
            }

            //soft delete the event
            singleEvent.IsActive = false;
            singleEvent.IsComplete = false;
            _repo.Update<Event>(singleEvent);
        }

        #region Methods to convert Utc time to Local time for viewing (database still has UTC time)
        public EventsViewModel convertDatesFromUtcToLocalTime(EventsViewModel vm)
        {
            foreach (var sglEvent in vm.Events)
            {
                sglEvent.EventStartDate = sglEvent.EventStartDate.ToLocalTime();
                sglEvent.EventEndDate = sglEvent.EventEndDate.ToLocalTime();
                sglEvent.DateCreated = sglEvent.DateCreated.ToLocalTime();
            }

            return vm;
        }

        public EventDetailsViewModel convertDatesFromUtcToLocalTime(EventDetailsViewModel sglEventVm)
        {
            sglEventVm.Event.EventStartDate = sglEventVm.Event.EventStartDate.ToLocalTime();
            sglEventVm.Event.EventEndDate = sglEventVm.Event.EventEndDate.ToLocalTime();
            sglEventVm.Event.DateCreated = sglEventVm.Event.DateCreated.ToLocalTime();

            return sglEventVm;
        }

        public Event convertDatesFromUtcToLocalTime(Event sglEventVm)
        {
            sglEventVm.EventStartDate = sglEventVm.EventStartDate.ToLocalTime();
            sglEventVm.EventEndDate = sglEventVm.EventEndDate.ToLocalTime();
            sglEventVm.DateCreated = sglEventVm.DateCreated.ToLocalTime();

            return sglEventVm;
        }

        public EventLocationViewModel convertDatesFromUtcToLocalTime(EventLocationViewModel sglEventVm)
        {
            sglEventVm.EventStartDate = sglEventVm.EventStartDate.ToLocalTime();
            sglEventVm.EventEndDate = sglEventVm.EventEndDate.ToLocalTime();
            sglEventVm.DateCreated = sglEventVm.DateCreated.ToLocalTime();

            return sglEventVm;
        }
        #endregion

        #region Methods to show user's name
        public EventsViewModel DisplayUserName(EventsViewModel vm)
        {
            foreach (var sglEvent in vm.Events)
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
                }
            }

            return vm;
        }

        public EventDetailsViewModel DisplayUserName(EventDetailsViewModel sglEventVm)
        {
            var userId = sglEventVm.Event.ApplicationUserId;
            var userFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.FirstName).FirstOrDefault();
            var userLastName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.LastName).FirstOrDefault();
            sglEventVm.Event.ApplicationUserId = userFirstName + " " + userLastName;

            foreach (var comment in sglEventVm.Event.Comments)
            {
                var commentWriterId = comment.ApplicationUserId;
                var commentWriterFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == commentWriterId).Select(au => au.FirstName).FirstOrDefault();
                comment.ApplicationUserId = commentWriterFirstName;
            }

            return sglEventVm;
        }

        public EventLocationViewModel DisplayUserName(EventLocationViewModel sglEventVm)
        {
            var userId = sglEventVm.ApplicationUserId;
            var userFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.FirstName).FirstOrDefault();
            var userLastName = _repo.Query<ApplicationUser>().Where(au => au.Id == userId).Select(au => au.LastName).FirstOrDefault();
            sglEventVm.ApplicationUserId = userFirstName + " " + userLastName;

            foreach (var comment in sglEventVm.Comments)
            {
                var commentWriterId = comment.ApplicationUserId;
                var commentWriterFirstName = _repo.Query<ApplicationUser>().Where(au => au.Id == commentWriterId).Select(au => au.FirstName).FirstOrDefault();
                comment.ApplicationUserId = commentWriterFirstName;
            }

            return sglEventVm;
        }
        #endregion

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

        #region Methods to convert to string for is Private, is compelte,  Active/Inactive (deleted/active)
        public string ConvertIsPrivateToYesOrNo(bool isPrivate)
        {
            string stringIsPrivate = "";

            if (isPrivate == true)
            {
                stringIsPrivate = "Private";
            }
            else
            {
                stringIsPrivate = "Public";
            }
            return stringIsPrivate;
        }

        public string ConvertIsCompleteToCompletedOrInProgress(bool isComplete)
        {
            string stringIsComplete = "";

            if (isComplete == true)
            {
                stringIsComplete = "Completed";
            }
            else
            {
                stringIsComplete = "In Progress";
            }
            return stringIsComplete;
        }

        public string ConvertIsActiveToActiveOrInactive(bool isActive)
        {
            string stringIsActive = "";

            if (isActive == true)
            {
                stringIsActive = "Active";
            }
            else
            {
                stringIsActive = "Inactive";
            }
            return stringIsActive;
        }

        #endregion
    }
}