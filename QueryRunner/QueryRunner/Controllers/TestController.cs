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

        private List<TestViewModel> TestQuestions;

        //Student view test
        [Authorize]
        public ActionResult ViewTest(int TestID)
        {
            //TODO: check if they can view test. aka doing test now
            //TODO: check if test is already answered.
            //TODO: check if time is over
            ModelTest curTest = _testStore.GetTest(TestID);
            List<ModelQuestion> questions = _questionStore.GetQuestionsByTest(TestID).ToList();
            List<TestViewModel> testQuestions = new List<TestViewModel>();
            int iCount = 1;
            questions.ForEach(cur =>
            {
                TestViewModel item = new TestViewModel(cur.QuestionID);
                item.QuestionText = cur.Instruction;
                item.chekced = false;
                item.QuestionNum = iCount;
                testQuestions.Add(item);
                iCount++;
            });
            Session["Questions"] = testQuestions;
            Session["curQuestionID"] = testQuestions.First().QuestionID;
            Session["curQuestionNum"] = 1;
            return View(testQuestions.First());
        }

        [Authorize]
        [HttpPost]
        public ActionResult ViewTest(TestViewModel testViewModel)
        {
            //TODO save to db
            List<TestViewModel> testQuestions = System.Web.HttpContext.Current.Session["Questions"] as List<TestViewModel>;
            TestViewModel nextQuestion = null;
            Object Num = System.Web.HttpContext.Current.Session["curQuestionNum"];
            Object ID = System.Web.HttpContext.Current.Session["curTestID"];
            int curQuestionNum = int.Parse(Num.ToString()) + 1;
            foreach (TestViewModel item in testQuestions)
            {
                if (item.QuestionID == int.Parse(ID.ToString()))
                {
                    testQuestions.ElementAt(testQuestions.IndexOf(item)).QuestionAnswer = testViewModel.QuestionAnswer;
                }
                if (item.QuestionNum == curQuestionNum)
                {
                    nextQuestion = item;
                }
            }
            Session["curQuestionNum"] = curQuestionNum;
            Session["Questions"] = testQuestions;
            if (nextQuestion == null)
            {
                return RedirectToAction("ViewTestResults", "Test", new { model = testQuestions });
            }
            return View(nextQuestion);
        }

        [Authorize]
        public ActionResult ViewTestResults(List<TestViewModel> model)
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