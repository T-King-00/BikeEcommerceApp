using project1.ServiceReference2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.ViewModel
{
    public class ItemViewModel

    {

        public Bike bikeObjToView { get; set; }
        public IEnumerable<ServiceReference2.ItemsReview> reviews { get; set; }
        public ServiceReference2.ItemsReview reviewobj { get; set; }

    }
}