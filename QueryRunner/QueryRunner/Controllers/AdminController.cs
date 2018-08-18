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

        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AdminAddViewModel model)
        {
          if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Admin");
            }
          _userStore.CreateUser(model.ToDataModel(), "Admin");
          return RedirectToAction("Index", "Admin");
        }

        // GET: AddAdmin
        public ActionResult Delete()
        {
            List<ModelUser> Admins = _userStore.GetAdminUsers();
            return View(Admins);
        }

        public ActionResult DeleteConfirm(string username)
        {
            return PartialView(_userStore.GetUser(username));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirm(ModelUser model)
        { 
          if (!ModelState.IsValid)
            {
                return RedirectToAction("Delete", "Admin");
            }
          List<ModelUserRole> UR = _userRoleStore.GetUserRolesByUser(model.Username).ToList();
          Helper.FEach(UR, x => _userRoleStore.DeleteUserRole(x)); 
          _userStore.DeleteUser(model);
          return RedirectToAction("Delete", "Admin");;
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