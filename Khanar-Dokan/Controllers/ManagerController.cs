using Khanar_Dokan.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Dynamic;
using System.Web.Mvc;
using System.Data.Entity;

namespace Khanar_Dokan.Controllers
{
    public class ManagerController : Controller
    {
        private KhanarDokanEntities obj = new KhanarDokanEntities();
        // GET: Manager

        

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public ActionResult UserList()
        {
            return View(obj.Users.ToList()); ;
        }

        public ActionResult UserListDetails(int id)
        {
            dynamic dy = new ExpandoObject();
            dy.userdetails = getuser(id);
            dy.invoicedetails = getinvoice(id);
            return View(dy);
        }

        public List<User>getuser(int id)
        {
            List<User> oneuser = obj.Users.Where(u => u.uid == id).ToList();
            return oneuser;
        }

        public List<Invoice> getinvoice(int id)
        {
            List<Invoice> oneinvoice = obj.Invoices.Where(u => u.uid == id).ToList();
            return oneinvoice;
        }

        [HttpPost]
        public ActionResult UserListDelete(int id)
        {
            var obj1 = obj.Users.Where(u => u.uid == id).FirstOrDefault();
            obj.Users.Remove(obj1);
            obj.SaveChanges();
            return RedirectToAction("UserList");
        }

        [HttpGet]
        public ActionResult OrderPlaced()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Pending").ToList());
        }

        
        public ActionResult ChangeStatusToPending(int id)
        {
            var obj1 = obj.Histories.Where(u => u.hid == id).First();
            obj1.hstatus = "Kitchen";
            obj.SaveChanges();
            return RedirectToAction("OrderPlaced");
        }

        public ActionResult CheifPending()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Kitchen").ToList());
        }

        public ActionResult Employees()
        {
            return View(obj.Employees.Where(u => u.etype != "Admin").ToList());
        }

        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            var obj1 = obj.Employees.Where(u => u.eid == id).FirstOrDefault();
            int nid = obj1.uid;
            obj.Employees.Remove(obj1);
            obj.SaveChanges();

            var obj2 = obj.Users.Where(u => u.uid == nid).FirstOrDefault();
            obj.Users.Remove(obj2);
            obj.SaveChanges();
            return RedirectToAction("Employees");
        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            Employee ei = obj.Employees.Where(x => x.eid == id).FirstOrDefault();
            ei.eid = id;

            return View(ei);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee ei, int id)
        {
            Employee einow = obj.Employees.Where(x => x.eid == id).FirstOrDefault();
            einow.eid = id;
            einow.etype = ei.etype;
            einow.esalary = ei.esalary;
            obj.SaveChanges();
            return RedirectToAction("Employees");
        }


        public ActionResult Comm()
        {
            return View(obj.Comments.ToList());
        }

        public ActionResult DeleteComment(int id)
        {
            var obj2 = obj.Comments.Where(u => u.cid == id).FirstOrDefault();
            obj.Comments.Remove(obj2);
            obj.SaveChanges();
            return RedirectToAction("Comm");
        }

        public ActionResult FoodItemList()
        {
            return View(obj.FoodItems.ToList());
        }

        public ActionResult AddFoodItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFoodItem(FoodItem fi)
        {
            if (ModelState.IsValid)
            {
                obj.FoodItems.Add(fi);
                obj.SaveChanges();
                return RedirectToAction("FoodItemList");            }
            else
            {
                ModelState.AddModelError("", "Some Error Occured!!");
            }
            return View(fi);
        }


        public ActionResult DeleteFoodItem(int id)
        {
            var obj3 = obj.FoodItems.Where(u => u.fid == id).FirstOrDefault();
            obj.FoodItems.Remove(obj3);
            obj.SaveChanges();
            return RedirectToAction("FoodItemList");
        }

        [HttpGet]
        public ActionResult EditFoodItem(int id)
        {
            FoodItem fi = obj.FoodItems.Where(x => x.fid == id).FirstOrDefault();
            fi.fid = id;

            return View(fi);
        }

        [HttpPost]
        public ActionResult EditFoodItem(FoodItem fi, int id)
        {
            FoodItem foodToUpdate = obj.FoodItems.Where(x => x.fid == id).FirstOrDefault();
            //foodToUpdate.fid = id;
            foodToUpdate.fname = fi.fname;
            foodToUpdate.fprice = fi.fprice;
            foodToUpdate.fdetails = fi.fdetails;
            foodToUpdate.fStatus = fi.fStatus;
            foodToUpdate.fimagefile = fi.fimagefile;
            foodToUpdate.catid = fi.catid;
            obj.SaveChanges();
            return RedirectToAction("FoodItemList");
        }

        public ActionResult HistoryAll()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Done").ToList());
        }

        public ActionResult HistoryDetails(int id)
        {
            dynamic dy = new ExpandoObject();
            dy.userdetails = getuser(id);
            dy.historydetails = gethistory(id);
            dy.invoicedetails = getinvoice(id);
            return View(dy);
        }

        public List<History> gethistory(int id)
        {
            List<History> oneuserHis = obj.Histories.Where(u => u.uid == id && u.hstatus == "Done").ToList();
            return oneuserHis;
        }

        public ActionResult HistoryDelete(int id)
        {
            var obj4 = obj.Histories.Where(u => u.hid == id).FirstOrDefault();
            obj.Histories.Remove(obj4);
            obj.SaveChanges();
            return RedirectToAction("HistoryAll");
        }

        public ActionResult ContactUs()
        {
            return View(obj.Contucts.Where(u => u.Comnstatus == "Pending").ToList());
        }

        public ActionResult ContactUsDone(int id)
        {
            var obj1 = obj.Contucts.Where(u => u.Comnid == id).First();
            obj1.Comnstatus = "Done";
            obj.SaveChanges();
            return RedirectToAction("ContuctUs");
        }

        public ActionResult DeleveryPending()
        {
            return View(obj.Histories.Where(u => u.hstatus == "Delivering").ToList());
        }

        public ActionResult fcal()
        {
            return View();
        }

        public ActionResult BestCustomer()
        {
            var studentName = obj.Histories.SqlQuery("SELECT DISTINCT CO.* FROM ( SELECT TOP(1) COS.uid, SUM(COS.hqty) as NoOfOrders FROM History AS COS GROUP BY COS.uid ORDER BY SUM(COS.hqty) DESC, uid  ASC ) AS COM INNER JOIN History AS CO ON COM.uid= CO.uid").ToList();
            return View(studentName);
        }
    }
}
 