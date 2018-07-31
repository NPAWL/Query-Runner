using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;

namespace QueryRunner.Controllers
{
    public class StudentController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;

        public StudentController() { }
        public StudentController(IUser userStore, IUserRole userRoleStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        // GET: TestStudent
        public ActionResult Index()
        {
            return View();
        }
    }
}