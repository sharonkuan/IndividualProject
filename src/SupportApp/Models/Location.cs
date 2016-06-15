﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupportApp.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string NameOfLocation { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public bool IsActive { get; set; }
    }
}
