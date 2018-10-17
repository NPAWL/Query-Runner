using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;

namespace QueryRunner.Controllers
{
    public class UserController : Controller
    {
        private IRole _roleStore;
        private IUser _userStore;
        private IUserRole _userRoleStore;

        public UserController() { }
        public UserController(IRole roleStore, IUser userStore, IUserRole userRoleStore)
        {
            _roleStore = roleStore;
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        // GET: User
        /*public ActionResult Index()
        {
            return View();
        }*/
    }
}