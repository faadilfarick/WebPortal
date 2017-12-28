﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string Organizer { get; set; }
    }
}