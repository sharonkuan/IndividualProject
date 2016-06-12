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

        public ICollection<EventUser> EventUsers { get; set; }
        //public ICollection<MemberVolunteer> MemberVolunteers { get; set; }
        //add an ICollection<ApplicationUser> ConnectedMembers for volunteer and members

        //need an identifier as type of user: members or volunteers

        //need an identifier public bool IsVolunteerApprovedByMember { get; set; }


    }
}
