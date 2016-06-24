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
    public class Event: AuditObj
    {
        //this can be events created by the admin
        //and can be used to create by members for private request
        public int Id { get; set; }
        [Required(ErrorMessage ="Event type is required")]
        public string EventType { get; set; }  //sales or stop over members' home/hospital
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
        public string IsVolunteerRequired { get; set; }  //if yes, ask preferred number of volunteer 
        public int PreferredNumberOfExpectedVolunteer { get; set; }

        public int NumberOfVolunteerRegistered { get; set; }
        public bool ApprovedByAdmin { get; set; }

        //public bool AcceptedByMember { get; set; }  //optional if event is a request from elder member
        //public ICollection<MemberVolunteer> MemberVolunteers { get; set; }

        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public int Views { get; set; }

        public ICollection<Location> Locations { get; set; }  //one to many locations
        public ICollection<Comment> Comments { get; set; }   //one to many comments for an event
        //TODO: Currently, it will display all events regardless its private or public 
        public ICollection<EventUser> EventUsers { get; set; }  //volunteers registered for many events

        public string ApplicationUserId { get; set; }

    }
}
