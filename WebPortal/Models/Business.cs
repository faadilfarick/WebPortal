using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Business
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }    
        public string Owner { get; set; }//in order to detect the original creator of the businness
    }
}