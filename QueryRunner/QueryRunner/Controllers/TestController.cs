using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;
using QueryRunner.Models;

namespace QueryRunner.Controllers
{
    public class TestController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;
        private ITest _testStore;
        private IQuestion _questionStore;

        public TestController() { }

        public TestController(IUser userStore, IUserRole userRoleStore, ITest testStore, IQuestion questionStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
            _testStore = testStore;
            _questionStore = questionStore;
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

        //Student view test
        [Authorize]
        public ActionResult ViewTest(int TestID)
        {
            //TODO: check if they can view test. aka doing test now
            //TODO: check if test is already answered.
            //TODO: check if time is over
            ModelTest curTest = _testStore.GetTest(TestID);
            List<ModelQuestion> questions = _questionStore.GetQuestionsByTest(TestID).ToList();
            questions.ForEach(cur =>
            {
                TestViewModel item = new TestViewModel(cur.QuestionID);
                item.QuestionText = cur.Instruction;
                item.chekced = false;
            });
            return View(questions);
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