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
        public DateTime EventDate { get; set; }
        public int StartHour { get; set; }
        public int StartMinutes { get; set; }
        public string StartTimeIsAmPm { get; set; }
        public int EndHour { get; set; }
        public int EndMinutes { get; set; }
        public string EndTimeIsAmPm { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsVolunteerRequired { get; set; }  //if yes, ask preferred number of volunteer 
        public int PreferredNumberOfExpectedVolunteer { get; set; }

        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Views { get; set; }



        public List<ApplicationUser> Volunteers { get; set; } 
        public List<Comment> Comments { get; set; }   
        public List<Location> Locations { get; set; }  
    }
}
