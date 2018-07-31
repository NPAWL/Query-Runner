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

        // GET: TestAdmin
        public ActionResult Index()
        {
            return View();
        }
    }
}