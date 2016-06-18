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

        //public EventDetailsViewModel ReloadEventDetailsPage(string userId, int id)
        //{
        //    var vm = new EventDetailsViewModel();
        //    var sglEvent = _repo.Query<Event>().Include(e => e.Locations).Include(e => e.Comments).FirstOrDefault(e => e.Id == id);

        //    if (userId != null)
        //    {
        //        vm.CanEdit = true;
        //    }
        //    else
        //    {
        //        vm.CanEdit = false;
        //    }
        //    vm.Event = sglEvent;
        //    return vm;  //usereventdetails
        //}


        public Event SaveEventComment(int id, string userId, Comment comment)
        {
            var selectedEvent = _repo.Query<Event>().Where(e => e.Id == id).Include(e => e.Comments).Include(e=>e.Locations).FirstOrDefault();

            comment.DateCreated = DateTime.UtcNow;
            comment.ApplicationUserId = userId;
            comment.IsActive = true;
            selectedEvent.Comments.Add(comment);
            _repo.SaveChanges();

            return selectedEvent;
        }
    }
}
