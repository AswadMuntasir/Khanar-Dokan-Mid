using Khanar_Dokan.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khanar_Dokan.Controllers
{
    public class CheifController : Controller
    {
        private KhanarDokanEntities obj = new KhanarDokanEntities();
        // GET: Cheif
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ManagerPending()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Pending").ToList());
        }

        [HttpGet]
        public ActionResult OrderPlaced()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Kitchen").ToList());
        }

        public ActionResult Done(int id)
        {
            var obj1 = obj.Histories.Where(u => u.hid == id).First();
            obj1.hstatus = "Delivering";
            obj.SaveChanges();
            return RedirectToAction("OrderPlaced");
        }

        [HttpGet]
        public ActionResult AllInventory()
        {
            return View(obj.Inventorys.ToList());
        }

        public ActionResult AddInventoryItem() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInventoryItem(Inventory inv)
        {
            if (ModelState.IsValid)
            {
                obj.Inventorys.Add(inv);
                obj.SaveChanges();
                return RedirectToAction("AllInventory");
            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }
            return View(inv);
        }

        public ActionResult DeleteInventory(int id)
        {
            var obj3 = obj.Inventorys.Where(u => u.iid == id).FirstOrDefault();
            obj.Inventorys.Remove(obj3);
            obj.SaveChanges();
            return RedirectToAction("AllInventory");
        }

        [HttpGet]
        public ActionResult EditInventory(int id)
        {
            Inventory inv = obj.Inventorys.Where(x => x.iid == id).FirstOrDefault();
            inv.iid = id;

            return View(inv);
        }

        [HttpPost]
        public ActionResult EditInventory(Inventory inv, int id)
        {
            Inventory InvUpdate = obj.Inventorys.Where(x => x.iid == id).FirstOrDefault();
            //foodToUpdate.fid = id;
            InvUpdate.iname = inv.iname;
            InvUpdate.istatus = inv.istatus;
            InvUpdate.iamount = inv.iamount;
            obj.SaveChanges();
            return RedirectToAction("FoodItemList");
        }
    }
}