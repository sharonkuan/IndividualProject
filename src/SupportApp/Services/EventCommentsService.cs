using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SupportApp.Data;
using SupportApp.Models;
using SupportApp.Repositories;
using SupportApp.ViewModels.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Services
{
    public class EventCommentsService : IEventCommentsService
    {
        private IGenericRepository _repo;

        public EventCommentsService(IGenericRepository repo, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
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

        public Event SaveEventComment(int id, string userId, Comment comment)
        {
            var selectedEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e => e.Locations).FirstOrDefault();

            comment.DateCreated = DateTime.UtcNow;
            comment.ApplicationUserId = userId;
            comment.IsActive = true;
            selectedEvent.Comments.Add(comment);
            _repo.SaveChanges();
            selectedEvent = convertDatesFromUtcToLocalTime(selectedEvent);
            selectedEvent = DisplayUserFirstName(selectedEvent);
            return selectedEvent;
        }
    }
}
