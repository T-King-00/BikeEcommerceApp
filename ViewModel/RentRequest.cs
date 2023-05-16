using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.ViewModel
{
    public class RentRequest
    {
        public Guid id { get; set; }
        public string bikeType { get; set; }

        public string dropoffcity { get; set; }
        public string pickupcity { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime dropoffDate { get; set; }
        public string othercomments{ get; set; }

    }
}