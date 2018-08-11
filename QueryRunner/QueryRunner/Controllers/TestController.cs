using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;

namespace QueryRunner.Controllers
{
    public class TestController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;

        public TestController() { }
        public TestController(IUser userStore, IUserRole userRoleStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        // GET: Tests
        public ActionResult CreateTest()
        {
            return View();
        }

        public ActionResult ViewTests()
        {
            return View();
        }

        public ActionResult ViewTest()
        {
            return View();
        }

        public ActionResult ViewTestResults()
        {
            return View();
        }

        public ActionResult Resources()
        {      
            return View();
        }

        public void exportStudentAnswers()
        {
            //need all answers here
            //loops through all answers
            //string name = lines[0].Username;
            //Helpers.Exporter.exportToTextFile(Response, new DataLayer.Entities.StudentAnswer[5]);
        }
    }
}