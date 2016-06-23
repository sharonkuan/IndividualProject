using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    public class Location : AuditObj
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Location name is required")]
        public string NameOfLocation { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        //public string CreatedBy { get; set; }
    }
}
