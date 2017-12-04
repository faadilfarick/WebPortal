using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPortal.Models
{
    public class Paymentinfo
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public int ItemPrice { get; set; }
        public int ItemQuantity { get; set; }


        public int cvv { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string cardnumber { get; set; }
        public string cardtype { get; set; }
        public int fee { get; set; }

        public int Subtotal { get; set; }
        public int Total { get; set; }
        public int Shipping { get; set; }
        public int Tax { get; set; }
    }
}