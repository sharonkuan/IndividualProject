using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Event
{
    public class PrivateEventsViewModel : EventsViewModel
    {
        public string MemberId { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberLastName { get; set; }

        public List<ApplicationUser> ConnectedVolunteers { get; set; }
        
        //below is in the ApplicationUser to identify if the volunteer connected with member
        //public bool IsVolunteerApprovedByMember { get; set; }
    }
}
