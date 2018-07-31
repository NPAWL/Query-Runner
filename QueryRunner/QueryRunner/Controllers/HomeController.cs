using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;

namespace QueryRunner.Controllers
{
    public class HomeController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;

        public HomeController() { }
        public HomeController(IUser userStore, IUserRole userRoleStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        public ActionResult FilterUserType(string username)
        {
            IQueryable<ModelUserRole> model = _userRoleStore.GetUserRolesByUser(username);
            if (model.Any(x => x.RoleName == "admin"))
              {
                return RedirectToAction("Index", "Admin");
              }
            else if (model.Any(x => x.RoleName == "student"))
              {
                return RedirectToAction("Index", "Student");
              }
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}