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

namespace QueryRunner.Controllers {
    public class TestController : Controller {
        private IUser _userStore;
        private IUserRole _userRoleStore;
        private ITest _testStore;
        private IQuestion _questionStore;
        private IStudentAnswer _studentAnswerStore;
        private IToken _tokenStore;

        public TestController() { }

        public TestController(IUser userStore, IUserRole userRoleStore, ITest testStore, IQuestion questionStore, IStudentAnswer studentAnswerStore, IToken tokenStore) {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
            _testStore = testStore;
            _questionStore = questionStore;
            _studentAnswerStore = studentAnswerStore;
            _tokenStore = tokenStore;
        }

        // GET: Tests
        public ActionResult CreateTest() {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTest(CreateTestViewModel model) {
            if (!ModelState.IsValid)
                return View(model);
            model.Questions = new List<CreateQuestionViewModel>();
            for (int i = 0; i < model.NumberOfQuestions; i++) {
                model.Questions.Add(new CreateQuestionViewModel(i + 1));
            }
            return View("CreateQuestion", model);
        }

        [HttpPost]
        public ActionResult CreateQuestion(CreateTestViewModel model) {
            foreach (CreateQuestionViewModel item in model.Questions) {
                if (!item.IsValid())
                    return View(model);
            }
            _testStore.CreateTest(model.ToDataModel(), User.Identity.Name, model.QuestionsToDataModel());
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult ViewStudentAccess(int TestID) {
            ModelTest curTest = _testStore.GetTest(TestID);
            ViewBag.Time = curTest.EndTime - curTest.StartTime;
            ViewBag.Date = curTest.Date;
            ViewBag.TestID = TestID;
            List<String> students = _tokenStore.GetTokensByTest(TestID).Select(cur => cur.Username).ToList();
            return View();
        }

        //Student view test
        [Authorize]
        public ActionResult ViewTest(int TestID) {
            ModelTest curTest = _testStore.GetTest(TestID);
            //may the user write the test?
            DateTime now = DateTime.Now;
            DateTime startTime = curTest.StartTime;
            DateTime endTime = curTest.EndTime;
            if (startTime > now || now > endTime)
                return Redirect(Request.UrlReferrer.ToString());
            //Check test completion
            String Username = User.Identity.Name;
            List<ModelStudentAnswer> curStud = _studentAnswerStore.GetStudentAnswersByStudentByTest(Username, TestID).ToList();

            List<ModelQuestion> questions = _questionStore.GetQuestionsByTest(TestID).ToList();
            List<StudentTestQuestionAnswerModel> testQuestions = new List<StudentTestQuestionAnswerModel>();
            int iCount = 1;
            questions.ForEach(cur => {
                StudentTestQuestionAnswerModel item = new StudentTestQuestionAnswerModel(cur.QuestionID);
                item.QuestionText = cur.Instruction;
                //init
                item.QuestionFlagged = false;
                item.QuestionNum = iCount;
                item.Username = User.Identity.Name;
                item.QuestionMark = cur.MaxMark;
                testQuestions.Add(item);
                iCount++;
            });

            Boolean found = false;
            foreach (ModelStudentAnswer cur in curStud) {
                //can maybe add to Token[ bool: TestSubmitted]
                if (cur.MarkObtained != 0 || !cur.Answer.Equals(""))
                    return RedirectToAction("ViewTestResults");
            }

            StudentTestQuestionAnswerModel.instance = testQuestions;
            return View(testQuestions.First());
        }

        [Authorize]
        [HttpPost]
        public ActionResult ViewTest(StudentTestQuestionAnswerModel testViewModel) {
            if (!ModelState.IsValid)
                return View(testViewModel);
            //  List<StudentTestQuestionAnswerModel> testQuestions = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            List<StudentTestQuestionAnswerModel> testQuestions = StudentTestQuestionAnswerModel.instance; //try this


            StudentTestQuestionAnswerModel nextQuestion = null;
            foreach (StudentTestQuestionAnswerModel item in testQuestions) {
                if (item.QuestionID == testViewModel.QuestionID) {
                    testQuestions.ElementAt(testQuestions.IndexOf(item)).QuestionAnswer = testViewModel.QuestionAnswer;
                }
                if (item.QuestionNum == testViewModel.QuestionNum + 1) {
                    nextQuestion = item;
                }
            }
            if (nextQuestion == null) {
                return SaveToDatabase(testQuestions);
            }
            this.ViewData = null;
            return View(nextQuestion);
        }

        [Authorize]
        public ActionResult ViewTestResults() {
            //Marking happens here
            //Update model to reflect mark
            return View(StudentTestQuestionAnswerModel.instance);
        }

        [Authorize] 
        [NonAction]
        public ActionResult SaveToDatabase(List<StudentTestQuestionAnswerModel> testViewModels) {
            testViewModels.ForEach(cur => _studentAnswerStore.CreateStudentAnswer(cur.ToDataModel()));
            return RedirectToAction("ViewTestResults");
        }

        public ActionResult Resources() {
            return View();
        }

        [HttpGet]
        public ActionResult SetStudentFeedback(String flags, String comment) {
            //TODO needs testing
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<List<int>>(flags);
            List<StudentTestQuestionAnswerModel> model = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            int TestID = _questionStore.GetQuestion(model.ElementAt(0).QuestionID).TestID;
            model.ForEach(cur => {
                foreach (int curFlag in obj) {
                    if (cur.QuestionID == curFlag) {
                        model.ElementAt(model.IndexOf(cur)).QuestionFlagged = true;
                        cur.QuestionFlagged = true;
                        break;
                    }
                }
            });

            List<ModelStudentAnswer> curAnswerList = _studentAnswerStore.GetStudentAnswersByStudentByTest(User.Identity.Name, TestID).ToList();
            curAnswerList.ForEach(curAnswer => {
                model.ForEach(curStudnet => {
                    if (curStudnet.QuestionID.Equals(curAnswer.QuestionID))
                        curAnswer.Flagged = curStudnet.QuestionFlagged;
                });
            });
            curAnswerList.ForEach(cur => _studentAnswerStore.UpdateStudentAnswer(cur));
            return Json(new { success = true, message = "Feedback saved!" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult exportStudentAnswers() {
            List<StudentTestQuestionAnswerModel> model = System.Web.HttpContext.Current.Session["Questions"] as List<StudentTestQuestionAnswerModel>;
            List<String> lines = new List<string>();
            lines.Add(String.Format("Username: {0}; Date: {1}", User.Identity.Name, DateTime.Now));
            lines.Add("\n<<<--------------------Questions-------------------->>>");
            model.ForEach(cur => lines.Add(cur.ToString()));
            //Helper.ExportToTextFile(Response, lines);
            MemoryStream memoryStream = Helper.ExportToTextFile(lines);
            return File(memoryStream.GetBuffer(), "text/plain", User.Identity.Name + ".gaadw.txt");

        }

    }
}