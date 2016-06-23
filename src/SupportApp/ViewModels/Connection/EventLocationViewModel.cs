using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Connection
{
    public class EventLocationViewModel
    {
        //Event
        public int EventId { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage = "Event type is required")]
        public string EventType { get; set; }  
        [Required(ErrorMessage = "Event title is required")]
        public string EventTitle { get; set; }
        public string Details { get; set; }
        [Required(ErrorMessage = "Event start date is required")]
        public DateTime EventStartDate { get; set; }
        [Required(ErrorMessage = "Event end date is required")]
        public DateTime EventEndDate { get; set; }
        public bool IsComplete { get; set; }
        public bool IsPrivate { get; set; }
        [Required(ErrorMessage = "Please confirm if volunteer is required")]
        public string IsVolunteerRequired { get; set; } 
        public int PreferredNumberOfExpectedVolunteer { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Views { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public string ApplicationUserId { get; set; }

        //public ICollection<Location> Locations { get; set; }
        //Location
        public int LocationId { get; set; }
        public bool LocationIsActive { get; set; }
        public DateTime LocationDateCreated { get; set; }
        [Required(ErrorMessage = "Location name is required")]
        public string NameOfLocation { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public bool CanEdit { get; set; }
        public bool HasClaim { get; set; }
        public string StringIsPrivate { get; set; }
        public string StringIsComplete { get; set; }
        public string stringIsActive { get; set; }
    }
}
