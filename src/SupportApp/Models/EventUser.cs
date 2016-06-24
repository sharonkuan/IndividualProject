using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    public class EventUser
    {
        //join table for event and application user
        //which tracks the volunteers for the events
        public Event Event { get; set; }
        public int EventId { get; set; }
        public ApplicationUser Member { get; set; }
        public string MemberId { get; set; }  

    }
}
