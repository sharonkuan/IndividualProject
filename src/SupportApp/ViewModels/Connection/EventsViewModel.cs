using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Connection
{
    /// <summary>
    /// returns lists of events in a single view model
    /// </summary>
    public class EventsViewModel
    {
        public List<Event> Events { get; set; }
        public List<ApplicationUser> Volunteers { get; set; } 
        
        //inside the Events already, no need
        //public ICollection<Comment> Comments { get; set; }   
        //public ICollection<Location> Locations { get; set; }

        public bool CanEdit { get; set; }
        public bool HasClaim { get; set; }
    }
}
