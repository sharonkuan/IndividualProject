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

        public EventCommentViewModel SaveEventComment(int id, string userId, Comment comment)
        {
            var eventCommentToSave = new EventCommentViewModel();
            var selectedEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).FirstOrDefault();

            eventCommentToSave.Comment.DateCreated = DateTime.UtcNow;
            comment.ApplicationUserId = userId;
            comment.IsActive = true;
            selectedEvent.Comments.Add(comment);

            _repo.SaveChanges();

            return eventCommentToSave;
        }
    }
}
