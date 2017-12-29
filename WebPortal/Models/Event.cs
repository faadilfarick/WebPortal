using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Event
    {
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public string Description { get; set; }
        public string Image { get; set; }
        public string Time { get; set; }
        public string Organizer { get; set; }
    }
}