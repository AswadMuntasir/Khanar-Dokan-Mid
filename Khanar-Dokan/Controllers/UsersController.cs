using Khanar_Dokan.Models.DataAccess;
using Khanar_Dokan.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khanar_Dokan.Controllers
{
    public class UsersController : Controller
    {
        private KhanarDokanEntities obj = new KhanarDokanEntities();
        // GET: Users
        [HttpGet]
        public ActionResult Index()
        {
            return View(obj.Users.ToList()); ;
        }

        public ActionResult Home()
        {
            return View(obj.FoodItems.ToList()); ;
        }

        public ActionResult ALHome()
        {
            return View(obj.FoodItems.ToList()); ;
        }

        public ActionResult Register()
        {
            Session.Clear();
            Session["UserID"] = null;
            Session["UserPassword"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Register(User usr)
        {
            if(ModelState.IsValid)
            {
                obj.Users.Add(usr);
                obj.SaveChanges();
                return RedirectToAction("Home");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }
            return View(usr);
        }

        public ActionResult Login()
        {
            Session.Clear();
            Session["UserID"] = null;
            Session["UserPassword"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel usr)
        {
            if (ModelState.IsValid)
            {
                var obj1 = obj.Users.Where(u => u.uemail.Equals(usr.uemail) && u.upassword.Equals(usr.upassword)).FirstOrDefault();
                if (obj1 != null)
                {
                    Session["UserID"] = obj1.uid.ToString();
                    Session["UserPassword"] = obj1.upassword.ToString();
                    if(Session["UserID"].ToString() == "5")
                    {
                        return RedirectToAction("Home", "Manager");
                    }
                    else if (Session["UserID"].ToString() == "8")
                    {
                        return RedirectToAction("Home", "Admin");
                    }
                    else if (Session["UserID"].ToString() == "9")
                    {
                        return RedirectToAction("Home", "Cheif");
                    }
                    else if (Session["UserID"].ToString() == "10")
                    {
                        return RedirectToAction("Home", "DeliveryBoy");
                    }
                    else
                    {
                        return RedirectToAction("ALHome", "Users");
                    }
                }
            }
           return View(usr);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session["UserID"] = null;
            Session["UserPassword"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult Menu()
        {
            return View(obj.FoodItems.ToList()); ;
        }
        public ActionResult About()
        {
            return View(obj.Comments.ToList());
        }
        public ActionResult FoodReview()
        {
            return View(obj.Comments.ToList());
        }

        public ActionResult ContactUs()
        {
            return View(); ;
        }

        public ActionResult ALMenu()
        {
            return View(obj.FoodItems.ToList()); ;
        }
        public ActionResult ALAbout()
        {
            return View(obj.Comments.ToList());
        }
        public ActionResult ALFoodReview()
        {
            return View(obj.Comments.ToList());
        }

        public ActionResult ALContactUs()
        {
            return View(); ;
        }

        public ActionResult History()
        {
            int id = Convert.ToInt32(Session["UserID"]);
            return View(obj.Histories.Where(u => u.uid == id).ToList());
        }

        [HttpPost]
        public ActionResult ALContactUs(Contuct con)
        {
            if (ModelState.IsValid)
            {
                obj.Contucts.Add(con);
                obj.SaveChanges();
                return RedirectToAction("ContactUs");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }

            return View(con);
        }


        [HttpPost]
        public ActionResult ContactUs(Contuct con)
        {
            if (ModelState.IsValid)
            {
                obj.Contucts.Add(con);
                obj.SaveChanges();
                return RedirectToAction("ContactUs");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }

            return View(con);
        }


        public ActionResult GiveFoodReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GiveFoodReview(Comment com)
        {
            int id = Int32.Parse(Session["UserID"].ToString());
            com.uid = id;
            History his = new History();

            if (ModelState.IsValid)
            {
                if (his.uid == id && com.fid == his.fid && his.hstatus == "Done")
                {
                    obj.Comments.Add(com);
                    obj.SaveChanges();
                    return RedirectToAction("FoodReview");
                }
                else
                {
                    ModelState.AddModelError("", "You did not orderd this item");
                }
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }
            return View(com);
        }

        public ActionResult AddToCart()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int id)
        {
            if(Session["cart"] == null)
            {
                List<ItemViewModel> cart = new List<ItemViewModel>();
                var food = obj.FoodItems.Find(id);
                cart.Add(new ItemViewModel()
                {
                    Food = food,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                List<ItemViewModel> cart = (List<ItemViewModel>)Session["cart"];
                var food = obj.FoodItems.Find(id);
                foreach(var item in cart)
                {
                    if(item.Food.fid == id)
                    {
                        int prevQenty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new ItemViewModel()
                        {
                            Food = food,
                            Quantity = prevQenty + 1
                        });
                        break;
                    }
                    else
                    {
                        cart.Add(new ItemViewModel()
                        {
                            Food = food,
                            Quantity = 1
                        });
                    }
                }
                Session["cart"] = cart;
            }
            return RedirectToAction("ALHome");
        }
        public ActionResult RemoveFromCart(int id)
        {
            List<ItemViewModel> cart = (List<ItemViewModel>)Session["cart"];
            foreach(var item in cart)
            {
                if(item.Food.fid == id)
                {
                    cart.Remove(item);
                    break;
                }
            }
            Session["cart"] = cart;
            return RedirectToAction("AddToCart");
        }

        public ActionResult Checkout()
        {
            return RedirectToAction("AddToCart");
        }

        [HttpPost]
        public ActionResult Checkout(Invoice inv)
        {
            List<ItemViewModel> cart = (List<ItemViewModel>)Session["cart"];
            History his = new History();
            //User us = new User();
            int userid = Int32.Parse(Session["UserID"].ToString());
            var us = obj.Users.Where(u => u.uid == userid).FirstOrDefault();


            if (ModelState.IsValid)
            {
                inv.uid = userid;
                inv.OrderDate = DateTime.Now;
                inv.Subtotal = Convert.ToDecimal(Session["totalprice"].ToString());
                inv.ShipMethod = "USP";
                inv.Shipping = 12.5000M;
                inv.SalesTax = 0.15M * inv.Subtotal;
                inv.Total = inv.Shipping + inv.SalesTax + inv.Subtotal;

            
                obj.Invoices.Add(inv);
                obj.SaveChanges();

                foreach (var item in cart)
                {
                    his.uid = userid;
                    his.fid = item.Food.fid;
                    his.saddress = us.uaddress;
                    his.sphone = us.uphone;
                    his.hdate = DateTime.Now;
                    his.hstatus = "Pending";
                    his.hqty = item.Quantity;
                    obj.Histories.Add(his);
                    obj.SaveChanges();
                }
                
                return RedirectToAction("History");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }

            return View(inv);
        }
    }
}