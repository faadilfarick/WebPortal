using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public string Producer { get; set; }
        public string Image { get; set; }

    }
}