using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;

namespace QueryRunner.Controllers
{
    public class AdminController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;

        public AdminController() { }
        public AdminController(IUser userStore, IUserRole userRoleStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        // GET: Admin home screen
        public ActionResult Index()
        {
            return View();
        }

        // GET: AddAdmin
        public ActionResult AddAdmin()
        {
            ViewBag.header = "Add new Admin";
            return View();
        }

        // GET: AddAdmin
        public ActionResult DelAdmin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult mAddAdmin(string returnUrl)//needs model to not lose context on screen
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Home/Index";
            }
            return RedirectToAction("AddAdmin", "Admin" );
            //return RedirectToLocal(returnUrl);

        }
    }
}