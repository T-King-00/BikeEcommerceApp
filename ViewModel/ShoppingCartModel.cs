using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace project1.ViewModel
{
    public class ShoppingCartModel
    {

        public List<project1.ServiceReference2.Bike> bikes;
        public List<project1.ServiceReference2.Part> parts;


        public ShoppingCartModel()
        {
            this.bikes = new List<project1.ServiceReference2.Bike>();
            this.parts = new List<project1.ServiceReference2.Part>();
        }






    }
}