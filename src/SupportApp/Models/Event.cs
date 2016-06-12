using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    /// <summary>
    /// Base elements of an event, tracks IsPrivate, IsVolunteerRequired,PreferredNumberOfExpectedVolunteer. Has one to many relationshiop between Locations and Comments. It has many to many relationship to users (volunteers)
    /// </summary>
    public class Event
    {
        //this can be events created by the admin
        //and can be used to create by members for private request
        public int Id { get; set; }
        [Required(ErrorMessage ="Event type is required")]
        public string EventType { get; set; }  //sales or stop over members' home/hospital
        [Required(ErrorMessage = "Event title is required")]
        public string EventTitle { get; set; }
        public string Details { get; set; }
        [Required(ErrorMessage = "Event date is required")]
        public DateTime EventDate { get; set; }
        [Range(0, 12,ErrorMessage ="Must be 0 to 12 hours")]
        public int StartHour { get; set; }
        [Range(0, 12, ErrorMessage = "Must be 0 to 60 minutes")]
        public int StartMinutes { get; set; }
        public string StartTimeIsAmPm { get; set; }
        [Range(0, 12, ErrorMessage = "Must be 0 to 12 hours")]
        public int EndHour { get; set; }
        [Range(0, 12, ErrorMessage = "Must be 0 to 60 minutes")]
        public int EndMinutes { get; set; }
        public string EndTimeIsAmPm { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsCompleted { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsVolunteerRequired { get; set; }  //if yes, ask preferred number of volunteer 
        public int PreferredNumberOfExpectedVolunteer { get; set; }
        //public ICollection<MemberVolunteer> MemberVolunteers { get; set; }

        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Views { get; set; }

        public ICollection<Location> Locations { get; set; }  //one to many locations
        public ICollection<Comment> Comments { get; set; }   //one to many comments for an event
        public ICollection<EventUser> EventUsers { get; set; }  //volunteers registered for many events
    }
}
