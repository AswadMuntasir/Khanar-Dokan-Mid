using Khanar_Dokan.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Khanar_Dokan.Controllers
{
    public class AdminController : Controller
    {
        KhanarDokanEntities kh = new KhanarDokanEntities();

        public ActionResult Salary()
        {
            return View(kh.Employees.ToList());
        }

        public ActionResult Home()
        {
            User us = kh.Users.FirstOrDefault();

            return View();
        }
        public ActionResult UserList()
        {
            return View(kh.Users.ToList());
        }
        public ActionResult Edit(int id)
        {
            User u = kh.Users.Where(item => item.uid == id).FirstOrDefault();
            return View(u);

        }

        [HttpPost]
        public ActionResult Edit(User u, int id)
        {
            User userToUpdate = kh.Users.Where(item => item.uid == id).FirstOrDefault();
            userToUpdate.uid = id;
            userToUpdate.uname = u.uname;
            userToUpdate.uemail = u.uemail;
            userToUpdate.uaddress = u.uaddress;
            kh.SaveChanges();
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            User userToDelete = kh.Users.Where(item => item.uid == id).FirstOrDefault();
            //kh.Users.Remove(userToDelete);
            return View(userToDelete);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            User userToDelete = kh.Users.Where(item => item.uid == id).FirstOrDefault();
            kh.Users.Remove(userToDelete);
            kh.SaveChanges();

            return RedirectToAction("UserList");
        }
        public ActionResult FoodItem()
        {
            return View(kh.FoodItems.ToList());
        }
        public ActionResult FoodEdit(int id)
        {
            FoodItem f = kh.FoodItems.Where(item => item.fid == id).FirstOrDefault();
            return View(f);

        }

        [HttpPost]
        public ActionResult FoodEdit(FoodItem f, int id)
        {
            FoodItem foodToUpdate = kh.FoodItems.Where(item => item.fid == id).FirstOrDefault();
            foodToUpdate.fid = id;
            foodToUpdate.fname = f.fname;
            foodToUpdate.fprice = f.fprice;
            foodToUpdate.fimagefile = f.fimagefile;
            kh.SaveChanges();
            return RedirectToAction("FoodItem");
        }
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(FoodItem f)
        {
            if (ModelState.IsValid)
            {
                kh.FoodItems.Add(f);
                kh.SaveChanges();
                return RedirectToAction("FoodItem");
            }
            return View(f);
        }
        public ActionResult FoodDelete(int id)
        {
            FoodItem foodToDelete = kh.FoodItems.Where(item => item.fid == id).FirstOrDefault();

            return View(foodToDelete);
        }
        [HttpPost, ActionName("FoodDelete")]
        public ActionResult FooditemDelete(int id)
        {
            FoodItem foodToDelete = kh.FoodItems.Where(item => item.fid == id).FirstOrDefault();
            kh.FoodItems.Remove(foodToDelete);
            kh.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult OrderPlaced()
        {
            return View(kh.Histories.Where(u => u.hstatus == "available").ToList());
        }

        public ActionResult HistoryAll()
        {
            return View(kh.Histories.Where(u => u.hstatus == "Done").ToList());
        }

    }
}