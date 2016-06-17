using SupportApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.ViewModels.Connection
{
    public class EventCommentViewModel
    {
        public Event Event { get; set; }
        public Comment Comment { get; set; }
        public bool CanEdit { get; set; }
    }
}
