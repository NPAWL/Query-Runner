using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace QueryRunner.Models {
    public class StudentTestQuestionAnswerModel {
        private ModelStudentAnswer _model = new ModelStudentAnswer();

        public static List<StudentTestQuestionAnswerModel> instance { get; set; }

        public string QuestionText { get; set; }
        public int QuestionNum { get; set; }
        public int QuestionMark { get; set; }
        public int QuestionID { get { return _model.QuestionID; } set { _model.QuestionID = value; } }
        public bool QuestionFlagged { get { return _model.Flagged; } set { _model.Flagged = value; } }
        [Required]
        public string QuestionAnswer { get { return _model.Answer; } set { _model.Answer = value; } }
        public string Username { get { return _model.Username; } set { _model.Username = value; } }
        public int MarkObtained { get { return _model.MarkObtained; } set { _model.MarkObtained = value; } }

        public ModelStudentAnswer ToDataModel() {
            return _model;
        }

        public StudentTestQuestionAnswerModel(ModelStudentAnswer model) {
            _model = model;
        }

        public StudentTestQuestionAnswerModel(int questionID) {
            QuestionID = questionID;
        }

        public StudentTestQuestionAnswerModel() {
        }

        public override string ToString() {
            StringBuilder bob = new StringBuilder();
            bob.AppendFormat("\nQuestion {0}({1})\n", QuestionNum, QuestionID);
            bob.AppendFormat("\t Answer: {0}\n", QuestionAnswer);
            bob.AppendFormat("\t Mark: {0}/{1}\n", MarkObtained, QuestionMark);
            bob.AppendFormat("\t Flagged: {0}\n", QuestionFlagged);
            return bob.ToString();
        }
    }

}