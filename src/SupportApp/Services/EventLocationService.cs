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

        public Event SaveEventLocation(int eventId, int locationId, Location location)
        {
            var selectedEvent = _repo.Query<Event>().Where(e => e.Id == eventId).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            if (locationId == 0)
            {
                location.DateCreated = DateTime.UtcNow;
                location.IsActive = true;
                selectedEvent.Locations.Add(location);
            }
            else
            {
                foreach (var place in selectedEvent.Locations)
                {
                    if (place.Id == locationId)
                    {
                        place.NameOfLocation = location.NameOfLocation;
                        place.Address = location.Address;
                        place.City = location.City;
                        place.State = location.State;
                        place.Zip = location.Zip;
                        place.IsActive = true;
                        break;
                    }
                }
            }
            _repo.SaveChanges();
            selectedEvent = convertDatesFromUtcToLocalTime(selectedEvent);
            selectedEvent = DisplayUserFirstName(selectedEvent);
            return selectedEvent;
        }
    }
}

