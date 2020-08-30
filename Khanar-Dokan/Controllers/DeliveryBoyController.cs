using Khanar_Dokan.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khanar_Dokan.Controllers
{
    public class DeliveryBoyController : Controller
    {
        private KhanarDokanEntities obj = new KhanarDokanEntities();
        // GET: DeliveryBoy
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrderPlaced()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Delivering").ToList());
        }

        public ActionResult Done(int id)
        {
            var obj1 = obj.Histories.Where(u => u.hid == id).First();
            obj1.hstatus = "Done";
            obj.SaveChanges();
            return RedirectToAction("OrderPlaced");
        }
    }
}