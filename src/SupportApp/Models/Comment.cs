using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    public class Comment: AuditObj
    {
        public int Id { get; set; }
        public string Message { get; set; }
        //only members can add comments
        //public ApplicationUser Member { get; set; }
        public string ApplicationUserId { get; set; }

    }
}
