using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Interfaces;
using Library.Models;
using QueryRunner.Models;
using QueryRunner.Helpers;

namespace QueryRunner.Controllers
{
    public class AdminController : Controller
    {
        private IUser _userStore;
        private IUserRole _userRoleStore;
        private ITest _testStore;
        private IQuestion _questionStore;
        private IStudentAnswer _studentAnswerStore;
        private IToken _tokenStore;

        public AdminController() { }
        public AdminController(IUser userStore, IUserRole userRoleStore, ITest testStore, IQuestion questionStore, IStudentAnswer studentAnswerStore, IToken tokenStore)
        {
            _userStore = userStore;
            _userRoleStore = userRoleStore;
            _testStore = testStore;
            _questionStore = questionStore;  
            _studentAnswerStore = studentAnswerStore;
            _tokenStore = tokenStore;
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
          _userStore.CreateUser(model.ToDataModel(), "admin");
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
          HelpingClass.FEach(UR, x => _userRoleStore.DeleteUserRole(x)); 
          _userStore.DeleteUser(model);
          return RedirectToAction("Delete", "Admin");;
        }

        public ActionResult ViewTests()
        {
            List<ModelTest> model = _testStore.ReadTests().ToList();
            return View(model);
        }

        public ActionResult EditTest(int ID)
        {
          List<ModelQuestion> questions = _questionStore.GetQuestionsByTest(ID).ToList();
          EditTestAndQuestionsViewModel model = new EditTestAndQuestionsViewModel(_testStore.GetTest(ID),questions);
          return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTestF(EditTestAndQuestionsViewModel model)
        {
          if (!ModelState.IsValid)
            {    
                return RedirectToAction("ViewTests", "Admin");
            }
          _testStore.UpdateTest(model.ToDataModel());   
          HelpingClass.FEach(model.Questions, x => _questionStore.UpdateQuestion(x));
          return RedirectToAction("ViewTests", "Admin");
        }

        public ActionResult DeleteTest(int ID)
        {
          DeleteTestViewModel model = new DeleteTestViewModel(_testStore.GetTest(ID));  
          return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTestF(DeleteTestViewModel model)
        {
          if (!ModelState.IsValid)
            {                                                 
                return RedirectToAction("ViewTests", "Admin");
            }
          _testStore.DeleteTest(model.ToDataModel());    
          return RedirectToAction("ViewTests", "Admin");
        }


        [HttpPost]
        public ActionResult mAddAdmin(string returnUrl)//needs model to not lose context on screen
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "/Home/Index";
            }
            return RedirectToAction("Add", "Admin");
            //return RedirectToLocal(returnUrl);

        }

        public ActionResult ViewMarks(int ID)
        {
            return RedirectToAction("ViewMarks", "Student", new { TestID = ID });
        }

        public ActionResult AddStudentsToTest(int ID)
        {         
            return View(new AddStudentsToTestModel(ID));
        }

        [HttpPost]
        public ActionResult AddStudentsToTest(AddStudentsToTestModel model)
        {         
            if (!ModelState.IsValid)
              {
                return View(model);
              } 
            ModelTest test = _testStore.GetTest(model.TestID);  
            Stream inStream = model.file.InputStream;      
            StreamReader sr = new StreamReader(inStream);
            try {
              List<string> usernames = new List<string>();
              while (!sr.EndOfStream)
                {
                  ModelUser user = new ModelUser();
                  string str = sr.ReadLine();
                  string[] userDetails = str.Split(',');
                  user.Username = userDetails[0];
                  user.PasswordHash = userDetails[1];
                  usernames.Add(userDetails[0]);
                  if (_userStore.GetUser(user.Username) == null)
                    _userStore.CreateUser(user,"student");  
                }
              sr.Close();
              ModelToken token = new ModelToken();
              token.CreatedTime = DateTime.Now;
              DateTime date = test.Date; DateTime time = test.EndTime; time.AddHours(1);
              date.AddHours(time.Hour);date.AddMinutes(time.Minute);
              token.ExpiredTime = date;
              _tokenStore.CreateToken(token,usernames.ToArray(),model.TestID);
              return RedirectToAction("ViewTests", "Admin");
            } catch (Exception e) {      
              sr.Close();
              return RedirectToAction("ViewTests", "Admin");
            }

        }
    }
}