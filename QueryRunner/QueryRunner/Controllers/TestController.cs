using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Library.Interfaces;
using Library.Models;
using QueryRunner.Helpers;
using QueryRunner.Models;

namespace QueryRunner.Controllers
{
    public class TestController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;
        private ITest _testStore;
        private IQuestion _questionStore;
        private IStudentAnswer _studentAnswerStore;
        private IToken _tokenStore;

        public TestController() { }

        public TestController(IUser userStore, IUserRole userRoleStore, ITest testStore, IQuestion questionStore, IStudentAnswer studentAnswerStore, IToken tokenStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
            _testStore = testStore;
            _questionStore = questionStore;
            _studentAnswerStore = studentAnswerStore;
            _tokenStore = tokenStore;
        }

        // GET: Tests
        public ActionResult CreateTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTest(CreateTestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            for (int i = 0; i < model.NumberOfQuestions; i++)
            {
                model.Questions.Add(new CreateQuestionViewModel());
            }
            model.QuestionNumber = 1;
            return View("CreateQuestion", model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(CreateTestViewModel model)
        {

            return View(model);
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
            List<StudentTestQuestionAnswerModel> testQuestions = new List<StudentTestQuestionAnswerModel>();
            int iCount = 1;
            questions.ForEach(cur =>
            {
                StudentTestQuestionAnswerModel item = new StudentTestQuestionAnswerModel(cur.QuestionID);
                item.QuestionText = cur.Instruction;
                item.QuestionFlagged = false;
                item.QuestionNum = iCount;
                item.Username = User.Identity.Name;
                item.QuestionMark = cur.MaxMark;
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
        public ActionResult ViewTest(StudentTestQuestionAnswerModel testViewModel)
        {
            List<StudentTestQuestionAnswerModel> testQuestions = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            StudentTestQuestionAnswerModel nextQuestion = null;
            if (!ModelState.IsValid)
                return View(testViewModel);
            Object Num = System.Web.HttpContext.Current.Session["curQuestionNum"];
            Object ID = System.Web.HttpContext.Current.Session["curQuestionID"];
            int curQuestionNum = int.Parse(Num.ToString()) + 1;
            foreach (StudentTestQuestionAnswerModel item in testQuestions)
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
                return SaveToDatabase(testQuestions);
            }
            Session["curQuestionID"] = nextQuestion.QuestionID;
            return View(nextQuestion);
        }

        [Authorize]
        public ActionResult ViewTestResults()
        {
            List<StudentTestQuestionAnswerModel> model = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            //Marking happens here
            //Update model to reflect mark
            return View(model);
        }

        [Authorize]
        public ActionResult SaveToDatabase(List<StudentTestQuestionAnswerModel> testViewModels)
        {
            testViewModels.ForEach(cur => _studentAnswerStore.CreateStudentAnswer(cur.ToDataModel()));
            return RedirectToAction("ViewTestResults", "Test");
        }

        public ActionResult Resources()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SetStudentFeedback(String flags, String comment)
        {
            //TODO needs testing
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<List<int>>(flags);
            List<StudentTestQuestionAnswerModel> model = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            int TestID = _questionStore.GetQuestion(model.ElementAt(0).QuestionID).TestID;
            model.ForEach(cur =>
            {
                foreach (int curFlag in obj)
                {
                    if (cur.QuestionID == curFlag)
                    {
                        model.ElementAt(model.IndexOf(cur)).QuestionFlagged = true;
                        cur.QuestionFlagged = true;
                        break;
                    }
                }
            });

            List<ModelStudentAnswer> curAnswerList = _studentAnswerStore.GetStudentAnswersByStudentByTest(User.Identity.Name, TestID).ToList();
            curAnswerList.ForEach(curAnswer =>
            {
                model.ForEach(curStudnet =>
                {
                    if (curStudnet.QuestionID.Equals(curAnswer.QuestionID))
                        curAnswer.Flagged = curStudnet.QuestionFlagged;
                });
            });
            curAnswerList.ForEach(cur => _studentAnswerStore.UpdateStudentAnswer(cur));
            return Json(new { success = true, message = "Feedback saved!" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult exportStudentAnswers()
        {
            List<StudentTestQuestionAnswerModel> model = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            List<String> lines = new List<string>();
            lines.Add(String.Format("Username: {0}; Date: {1}", User.Identity.Name, DateTime.Now));
            lines.Add("\n<<<--------------------Questions-------------------->>>");
            model.ForEach(cur => lines.Add(cur.ToString()));
            //Helper.ExportToTextFile(Response, lines);
            MemoryStream memoryStream = Helper.ExportToTextFile(lines);
            return File(memoryStream.GetBuffer(), "text/plain", User.Identity.Name + ".gaadw.txt");

        }

        [HttpGet]
        public ActionResult exportAllStudentMarks()
        {
            int TestID = 0;
            List<ModelQuestion> questionsRAW = _questionStore.GetQuestionsByTest(TestID).ToList();
            List<ModelStudentAnswer> studentAnswers = _studentAnswerStore.GetStudentAnswersByTest(TestID).ToList();

            Dictionary<int, int> questions = new Dictionary<int, int>();
            questionsRAW.ForEach(cur => questions.Add(cur.QuestionID, cur.MaxMark));

            Dictionary<String, Double> marks = new Dictionary<string, double>();
            studentAnswers.ForEach(cur =>
            {
                double curMark = marks[cur.Username];
                double mark = cur.MarkObtained / (questions[cur.QuestionID]);
                marks[cur.Username] = (curMark + mark);
            });

            List<String> lines = new List<string>();
            foreach (KeyValuePair<String, Double> entry in marks)
            {
                marks[entry.Key] = entry.Value / questions.Count;
                lines.Add(String.Format("Username: {0}; Mark: {1}", entry.Key, marks[entry.Key]));
            }
            //Helper.ExportToTextFile(Response, lines);
            MemoryStream memoryStream = Helper.ExportToTextFile(lines);
            return File(memoryStream.GetBuffer(), "text/plain", User.Identity.Name + ".gaadw.txt");

        }
    }
}