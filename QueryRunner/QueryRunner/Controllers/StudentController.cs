using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;
using QueryRunner.Models;
using QueryRunner.Helpers;

namespace QueryRunner.Controllers
{
    public class StudentController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;
        private IToken _tokenStore;
        private ITest _testStore;
        private IQuestion _question;
        private IStudentAnswer _studentAnswer;

        public StudentController() { }

        public StudentController(IUser userStore, IUserRole userRoleStore, IToken tokenStore, ITest testStore, IQuestion question, IStudentAnswer studentAnswer)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
            _tokenStore = tokenStore;
            _testStore = testStore;
            _question = question;
            _studentAnswer = studentAnswer;
        }



        // GET: TestStudent
        [Authorize]
        public ActionResult Index()
        {
            List<DisplayStudentTestsModel> testList = new List<DisplayStudentTestsModel>();
            try
            {
                IToken temp = _tokenStore;
                List<ModelToken> curToken = _tokenStore.GetTokensByUsername(User.Identity.Name).ToList();
                curToken.ForEach(token =>
                {
                    DisplayStudentTestsModel cur = new DisplayStudentTestsModel(token.TestID);
                    ModelTest curTest = _testStore.GetTest(token.TestID);
                    cur.TestName = curTest.TestName;
                    cur.Date = curTest.StartTime;

                    String markStud = SumStudAnswerMarks(_studentAnswer.GetStudentAnswersByStudentByTest(User.Identity.Name, token.TestID)) ?? "n/a";
                    String markTest = "";
                    if (!markStud.Equals("n/a"))
                        markTest = SumTestMarks(_question.GetQuestionsByTest(token.TestID)) ?? "n/a";
                    cur.Mark = markStud + " / " + markTest;
                    testList.Add(cur);
                });

            }
            catch (NullReferenceException e)
            {
                return View();
            }

            return View(testList);


        }

        public ActionResult ListStudents()
        {
            return View();
        }

        public ActionResult GotoTest(int TestID)
        {
            return RedirectToAction("ViewTest", "Test", new { TestID = TestID});
        }

        private String SumTestMarks(IQueryable<ModelQuestion> list)
        {
            int sum = 0;
            try
            {
                List<ModelQuestion> temp = list.ToList<ModelQuestion>();
                if (list.Count() == 0) return null;
                foreach (ModelQuestion cur in list)
                {
                    sum += cur.MaxMark;
                }
                return sum.ToString();
            }
            catch (NotSupportedException e)
            {
                return null;
            }
        }

        private String SumStudAnswerMarks(IQueryable<ModelStudentAnswer> list)
        {
            int sum = 0;
            try
            {
                List<ModelStudentAnswer> temp = list.ToList<ModelStudentAnswer>();
                if (list == null || list.Count() == 0) return null;
                foreach (ModelStudentAnswer cur in list)
                {
                    sum += cur.MarkObtained;
                }
                return sum.ToString();
            }
            catch (NotSupportedException e)
            {
                return null;
            }
        }
    }
}