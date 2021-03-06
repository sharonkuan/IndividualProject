﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SupportApp.Data;
using SupportApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SupportApp.Services;
using SupportApp.ViewModels.Connection;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SupportApp.API
{
    [Route("api/[controller]")]
    public class EventCommentController : Controller
    {
        private IEventCommentsService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventCommentController(IEventCommentsService service, UserManager<ApplicationUser> userManager)
        {
            this._service = service;
            this._userManager = userManager;
        }

        [HttpPost("{id}")]
        [Authorize]
        public IActionResult Post(int id, [FromBody]Comment comment)
        {
            var addedCommentEvent = new Event();
            
            if (comment == null || String.IsNullOrWhiteSpace(comment.ToString()))
            {
                ModelState.AddModelError("Message", "Comment is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var userId = _userManager.GetUserId(User);

            if (userId != null && comment.Id == 0)
            {
                addedCommentEvent = _service.SaveEventComment(id, userId, comment);
            }
       
            return Ok(addedCommentEvent);
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // PUT api/event/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody]bool adminApproved)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = _userManager.GetUserId(User);
            
            _service.SignUpToVolunteer(userId, id, adminApproved);
            return Ok();
        }

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var selectedEventComments = _db.Events.Where(e => e.Id == id).Include(e => e.Comments).Select(e => e.Comments).ToList();

        //    if (id == 0 || selectedEventComments == null)
        //    {
        //        return BadRequest();
        //    }

        //    foreach (Comment comment in selectedEventComments)
        //    {
        //        _db.Comments.Remove(comment);
        //    };

        //    _db.SaveChanges();
        //    return Ok();
        //}
    }
}
