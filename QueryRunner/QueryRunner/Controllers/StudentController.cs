using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;
using QueryRunner.Models;
using QueryRunner.Helpers;
using System.Threading;
using System.IO;

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

        public ActionResult ViewMarks(int TestID)
        {
            return View(getMarksModel(TestID));
        }

        private List<ViewStudentMarks> getMarksModel(int TestID)
        {
            Dictionary<int, int> questions = new Dictionary<int, int>();
            _question.GetQuestionsByTest(TestID).ToList().ForEach(cur => questions.Add(cur.QuestionID, cur.MaxMark));

            Dictionary<String, Double> marks = new Dictionary<string, double>();
            _studentAnswer.GetStudentAnswersByTest(TestID).ToList().ForEach(cur =>
            {
                double curMark = 0;
                if (marks.Keys.Contains(cur.Username))
                    curMark = marks[cur.Username];
                double mark = (cur.MarkObtained * 1.000) / (questions[cur.QuestionID]);
                marks[cur.Username] = (curMark + mark);
            });

            List<ThreadStart> runners = new List<ThreadStart>();
            List<ViewStudentMarks> model = new List<ViewStudentMarks>();
            foreach (KeyValuePair<String, Double> entry in marks)
            {
                runners.Add(new ThreadStart(() =>
                {
                    marks[entry.Key] = Math.Round(entry.Value / questions.Count * 100, 2);
                    model.Add(new ViewStudentMarks(TestID, entry.Key, marks[entry.Key]));
                }));
            }
            runners.ForEach(cur => new Thread(cur).Start());
            Session["modelViewStudentMarks"] = model;
            return model;
        }

        public ActionResult GotoTest(int TestID)
        {
            return RedirectToAction("ViewTest", "Test", new { TestID = TestID });
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

        [HttpGet]
        public ActionResult exportAllStudentMarks(String ID)
        {
            List<ViewStudentMarks> marksies = System.Web.HttpContext.Current.Session["modelViewStudentMarks"] as List<ViewStudentMarks>;
            if (marksies == null)
                marksies = getMarksModel(int.Parse(ID));

            List<String> lines = new List<string>();
            lines.Add(String.Format("Username: {0}; Date: {1}\n", User.Identity.Name, DateTime.Now));
            lines.Add("Username\tMark\n");
            marksies.ForEach(cur => lines.Add(String.Format("{0}\t{1}\n", cur.Name, cur.Presentage).ToString()));

            MemoryStream memoryStream = Helper.ExportToTextFile(lines);
            return File(memoryStream.GetBuffer(), "text/plain", User.Identity.Name + ".txt");

        }
    }
}