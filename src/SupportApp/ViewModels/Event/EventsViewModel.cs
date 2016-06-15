using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Event
{
    public class EventsViewModel
    {
        public int Id { get; set; }
        public string EventType { get; set; } 
        public string EventTitle { get; set; }
        public string Details { get; set; }
        public DateTime EventStartDate { get; set; }
        public DateTime EventEndDate { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
        public bool IsComplete { get; set; }

        public bool IsPrivate { get; set; }
        public string IsVolunteerRequired { get; set; }  //if yes, ask preferred number of volunteer 
        public int PreferredNumberOfExpectedVolunteer { get; set; }

        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Views { get; set; }

        public List<ApplicationUser> Volunteers { get; set; } 
        public ICollection<Comment> Comments { get; set; }   
        public ICollection<Location> Locations { get; set; }

        public bool CanEdit { get; set; }
    }
}
