using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SupportApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //contacts in the Contact table are registered for event volunteer
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrimaryPhone { get; set; }
        public bool IsProvider { get; set; }  //is yes, then member is a provider, else can be a member
        public bool WillingToVolunteer { get; set; }  //ifmembers willing to volunteer

        //tracks the events and volunteers for the events
        public ICollection<EventUser> EventUsers { get; set; }
        //connects the volunteers and members
        //this helps identify if members and volunteers are connected
        public ICollection<MemberVolunteer> MemberVolunteers { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}
