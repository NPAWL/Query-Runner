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
        public StudentController(IUser userStore, IUserRole userRoleStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
        }

        // GET: TestStudent
        [Authorize]
        public ActionResult Index()
        {
            List<DisplayStudentTestsModel> testList = new List<DisplayStudentTestsModel>();
            try
            {
                List<ModelToken> curToken = _tokenStore.GetTokensByUsername(User.Identity.Name).ToList();
                curToken.ForEach(token =>
                {
                    DisplayStudentTestsModel cur = new DisplayStudentTestsModel(token.TestID);
                    ModelTest curTest = _testStore.GetTest(token.TestID);
                    cur.TestName = curTest.TestName;
                    cur.Date = curTest.StartTime;
                    cur.Mark = SumStudAnswerMarks(_studentAnswer.GetStudentAnswersByTest(token.TestID)) + " / " + SumTestMarks(_question.GetQuestionsByTest(token.TestID));
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

        private String SumTestMarks(IQueryable<ModelQuestion> list)
        {
            int sum = 0;
            Helper.FEach(list, x => sum += x.MaxMark);
            return sum.ToString();
        }

        private String SumStudAnswerMarks(IQueryable<ModelStudentAnswer> list)
        {
            int sum = 0;
            Helper.FEach(list, x => sum += x.MarkObtained);
            return sum.ToString();
        }
    }
}