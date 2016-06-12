using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    /// <summary>
    /// This connects members and volunteers. Many to Many relationship
    /// </summary>
    public class MemberVolunteer
    {
        public ApplicationUser Member { get; set; }
        public string MemberId { get; set; }

        public ApplicationUser Volunteer { get; set; }
        public string VolunteerId { get; set; }
    }
}
