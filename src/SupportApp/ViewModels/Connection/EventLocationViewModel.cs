using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Connection
{
    public class EventLocationViewModel
    {
        public Event Event { get; set; }
        public Location Location { get; set; }
        public bool CanEdit { get; set; }
        public bool HasClaim { get; set; }
    }
}
