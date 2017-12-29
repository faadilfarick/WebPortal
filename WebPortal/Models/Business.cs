using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Business
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }    
        public string Owner { get; set; }//in order to detect the original creator of the businness
    }
}