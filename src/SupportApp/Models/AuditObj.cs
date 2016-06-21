using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    abstract public class AuditObj
    {
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        //public DateTime DateModified { get; set; }
        //public string LastModifiedByUserId { get; set; }
        //public DateTime DateDeleted { get; set; }
        //public string DeletedByUserId { get; set; }
    }
}
