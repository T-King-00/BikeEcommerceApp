using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.ServiceReference2;
using project1.ViewModel;

namespace project1.Controllers
{
    public class ShopController : Controller
    {
        ServiceReference2.BikeServiceClient serviceBikes = new ServiceReference2.BikeServiceClient();


        // GET: Shop
        public ActionResult Bikes()
        {
            IEnumerable<ServiceReference2.Bike> bikes=serviceBikes.GetBikes();

            return View(bikes);
        }


        public ActionResult Parts()
        {
            IEnumerable<ServiceReference2.Part> ps =serviceBikes.GetParts();

            return View(ps);
        }

        public ActionResult bikeDetail(Guid id)
        {

            ItemViewModel bikeModel = new ItemViewModel();
            bikeModel.bikeObjToView= serviceBikes.GetBike(id);
            bikeModel.reviews=serviceBikes.getReviews(id);

            
            
            return View(bikeModel);
        }
        public ActionResult partDetail(Guid id)
        {
            //ServiceReference2.BikeServiceClient serviceBikes = new ServiceReference2.BikeServiceClient();
            Part bikeObjToView = serviceBikes.GetPart(id);

            return View(bikeObjToView);
        }

 

        public ActionResult AddToCart(Guid ItemID,string fromWhere)
        {
            bool cameFromBikes = false;
            bool cameFromParts = false;
            try
            {
               
                if (Session["loggedIn"].ToString() == "true")
                {
                    ServiceReference2.BikeServiceClient serviceObj1 = new BikeServiceClient();

                    HttpContext.Session["counterOfItemsCart"] 
                        = Int32.Parse(HttpContext.Session["counterOfItemsCart"].ToString()) + 1;

                    ShoppingCartModel model = new ShoppingCartModel();
                    if (Session["ItemsCart"] != null)
                    {
                        model = Session["ItemsCart"] as ShoppingCartModel;
                    }

                    if (fromWhere=="parts")
                    {
                        Part newPart = serviceObj1.GetPart(ItemID);
                        model.parts.Add(newPart);
                        Session["ItemsCart"] = model;
                        return RedirectToAction("Parts");

                    }
                    else if (fromWhere == "bikes")
                    {
                        Bike newBike = serviceObj1.GetBike(ItemID);
                        model.bikes.Add(newBike);
                        Session["ItemsCart"] = model;
                        return RedirectToAction("Bikes");

                    }
                    




                    //get bike details by using service and then adds it to session object.
            /*        ServiceReference2.BikeServiceClient serviceObj = new BikeServiceClient();

                    if (serviceObj.GetBike(ItemID) != null)
                    {
                        Bike newBike = serviceObj.GetBike(ItemID);
                        HttpContext.Session["counterOfItemsCart"] = Int32.Parse(HttpContext.Session["counterOfItemsCart"].ToString()) + 1;
                        List<Bike> b = new List<Bike>();
                        if (Session["ItemsCart"] != null)
                        {
                            b = Session["ItemsCart"] as List<Bike>;
                        }
                        b.Add(newBike);
                        Session["ItemsCart"] = b.ToList();
                    }*/
               




                   
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction("LoginPage", "Home");
             
            }

        

    

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RemoveFromCart(Guid id)
        {

            //code logic here 
            return RedirectToAction("Cart");
        }

        public ActionResult Cart()
        {
            var cart = Session["ItemsCart"] as ShoppingCartModel;
            return View(cart);
        }


        public ActionResult Rent_Bike()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Rent_Bike(string bikeType, string city, Nullable<System.DateTime> PickUpDate, Nullable<System.DateTime>  DropInDate)
        {
            
            return RedirectToAction("Index","Home");
        }



        [HttpPost]
        public ActionResult addReview(ItemsReview rev,string rating)

        {
            rev.rating=Int32.Parse(rating.Substring(0,1));
            rev.itemId = ViewBag.itemid ;
            serviceBikes.addReviews(rev);

            return RedirectToAction("bikeDetail");
        }

    }
}