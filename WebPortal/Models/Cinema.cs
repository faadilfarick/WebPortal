using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Cinema
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int Seats { get; set; }//Number of seats
        public List<NowScreening> Movies { get; set; }
        public List<ScreeningTime> Time { get; set; }

    }

    public class ScreeningTime
    {
        public int ID { get; set; }
        public string Time { get; set; }
        public bool Checked { get; set; }
    }

    public class NowScreening
    {
        public int ID { get; set; }
        public string MovieName { get; set; }
        public bool Checked { get; set; }
    }
}