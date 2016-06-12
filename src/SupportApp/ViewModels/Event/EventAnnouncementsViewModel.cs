using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Event
{
    public class EventAnnouncementsViewModel
    {
        //announcement is showned only for public events
        public string EventType { get; set; }
        public string EventTitle { get; set; }
        public string Details { get; set; }
        public DateTime EventDate { get; set; }
        public int StartHour { get; set; }
        public int StartMinues { get; set; }
        public string StartTimeIsAmPm { get; set; }
        public int EndHour { get; set; }
        public int EndMinues { get; set; }
        public string EndTimeIsAmPm { get; set; }

        public bool IsVolunteerRequired { get; set; }  //if yes, ask preferred number of volunteer 
        public int PreferredNumberOfExpectedVolunteer { get; set; }
    }
}
