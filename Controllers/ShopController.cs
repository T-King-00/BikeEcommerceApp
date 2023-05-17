using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.ServiceReference1;
using project1.ServiceReference2;
using project1.ViewModel;

namespace project1.Controllers
{
    public class ShopController : Controller
    {
        ServiceReference2.BikeServiceClient serviceBikes = new ServiceReference2.BikeServiceClient();
        ServiceReference1.WebService1SoapClient serviceUser = new WebService1SoapClient();


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
            ItemViewModel partModel = new ItemViewModel();
            partModel.PartObjToView = serviceBikes.GetPart(id);
            partModel.reviews = serviceBikes.getReviews(id);

          

            return View(partModel);
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
            var cart = Session["ItemsCart"] as ShoppingCartModel;
            var partToRemove = cart.parts.Find(c => c.id == id);

            cart.parts.Remove(partToRemove);
            Session["ItemsCart"] = cart;


            var cart2 = Session["ItemsCart"] as ShoppingCartModel;
            var bikeToRemove = cart.bikes.Find(c => c.id == id);

            cart2.bikes.Remove(bikeToRemove);
            Session["ItemsCart"] = cart2;





            //code logic here 
            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
        {
            var cart = Session["ItemsCart"] as ShoppingCartModel;
            return View(cart);
        }




        public ActionResult Rent_Bike(RentRequest req)
        {
            return View(req);
        }
        [HttpPost]
        public ActionResult Rent_BikePOST(RentRequest objRentRequest)
        {
            SqlConnection conn = new SqlConnection("Data Source=TONYRIAD;Initial Catalog=BikeShopDb;Integrated Security=True");
            string cmdText;
            cmdText = "insert into [BikeShopDb].[dbo].[rentBikesRequests] " +
                      "([bikeType],[pickuploc],[dropoffloc],[pickUpDate],[DropOffDate],[otherComments])"
                      +"values (@p1,@p2,@p3,@p4,@p5,@p6);";

            conn.Open();
            SqlParameter parameter1 = new SqlParameter("@p1", objRentRequest.bikeType);
            SqlParameter parameter2 = new SqlParameter("@p2", objRentRequest.pickupcity);
            SqlParameter parameter3 = new SqlParameter("@p3", objRentRequest.dropoffcity);
            SqlParameter parameter4 = new SqlParameter("@p4", objRentRequest.PickupDate);
            SqlParameter parameter5 = new SqlParameter("@p5", objRentRequest.dropoffDate);
            SqlParameter parameter6 = new SqlParameter("@p6", objRentRequest.othercomments);

            SqlCommand command = new SqlCommand(cmdText, conn);
            command.Parameters.Add(parameter1);
            command.Parameters.Add(parameter2);
            command.Parameters.Add(parameter3);
            command.Parameters.Add(parameter4);
            command.Parameters.Add(parameter5);
            command.Parameters.Add(parameter6);
            command.ExecuteNonQuery();
            return RedirectToAction("RequestedRent","Shop");
        }
        public ActionResult RequestedRent()
        {
            return View();
        }
    

        public ActionResult order_done()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Checkoutpost()
        {

            serviceUser.paywithcredit("true");

            return RedirectToAction("order_done");
        }   

        public ActionResult Checkout()
        {

            
            return View();
        }
        [HttpPost]
        public ActionResult addReview(ItemsReview rev,string rating,Guid id)
        {
            if (Session["loggedIn"]=="true")
            {
                rev.rating = Int32.Parse(rating.Substring(0, 1));
                rev.userName = Session["userName"].ToString();

                rev.itemId = id;

                Guid idUser = (Guid)Session["userID"];
                rev.userID = idUser;

                serviceBikes.addReviews(rev);

                return RedirectToAction("Bikes");
            }
            else
            {
                return RedirectToAction("RegisterPage", "Home");
            }
            
            

        }

    }
}