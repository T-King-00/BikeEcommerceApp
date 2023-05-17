using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using project1.ServiceReference1;

namespace project1.Controllers
{
    public class HomeController : Controller
    {
        private Boolean loggedIn=false;
        
        public ActionResult Index()
        {
            return View();
         
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult LoginPage(ServiceReference1.User user)
        {
            ViewBag.Message = " LoginPage page.";
            
            return View(user);
        }

        public ActionResult Login(ServiceReference1.User user)
        {
            user.UserName = Request["UserName"];
            user.password = Request["password"];
            ServiceReference1.WebService1SoapClient service = new WebService1SoapClient();
            Guid id = service.Login(user.UserName, user.password);
            if (id!=Guid.Empty && id!=null )
            {
                Session["loggedIn"] = "true";
                Session["userID"] = id;
                Session["userName"] = user.UserName;
                loggedIn = true;
                HttpContext.Session["counterOfItemsCart"] = 0;
                HttpContext.Session["ItemsCart"] = null;
                
               
                return RedirectToAction("Index");
            }
            else
            {
                HttpContext.Session["loggedIn"] = "false";

            }


            return null;
        }

        public ActionResult Logout()
        {
            Session["loggedIn"] = "false";
            HttpContext.Session["counterOfItemsCart"] = 0;
            HttpContext.Session["ItemsCart"] = null;

            return RedirectToAction("Index");
        }

        public ActionResult RegisterPage(string msg)
        {
            ViewBag.registered = msg;

            return View();
        }
        [HttpPost]
        public ActionResult Register([Bind(Include = "firstName,lastName,UserName,email,password,gender,address,city")] ServiceReference1.User userToRegister)
        {
          
            ServiceReference1.WebService1SoapClient obj = new WebService1SoapClient();
            if (obj.Register(userToRegister))
            {
             
                return RedirectToAction("Index");
            }
            
            
            ViewBag.registered = "Failed to register";
            return RedirectToAction("RegisterPage",new{msg=ViewBag.registered});
            
        }


        public ActionResult EditProfile()
        {
            ServiceReference1.User userToEdit = new ServiceReference1.User();

            ServiceReference1.WebService1SoapClient service = new WebService1SoapClient();
            userToEdit=service.ViewUserDetails(Session["userName"].ToString());

            return View(userToEdit);
        }

        [HttpPost]
        public ActionResult EditProfilePOST([Bind(Include = "userName,firstName,lastName,email,gender,address,city")] ServiceReference1.User userToEdit)
        {

            ServiceReference1.WebService1SoapClient obj = new WebService1SoapClient();

            if (obj.UpdateUserDetails(userToEdit))
            {
                
                return RedirectToAction("Index");
            }
            ViewBag.edited = "Failed to edit, check all fields";
            return RedirectToAction("EditProfile", new { msg = ViewBag.edited });

    


        }

    }
}