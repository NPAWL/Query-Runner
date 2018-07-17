using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelStudentAnswer
    {
        public int SAID { get; set; }
        public int QuestionID { get; set; }
        public string Username { get; set; }
        public string Answer { get; set; }
        public bool Flagged { get; set; }
        public short Type { get; set; }
        public bool StudentAnswerActive { get; set; }

        internal IQueryable<ModelStudentAnswer> Get(QueryRunnerEntities context)
        {
            return from studentanswer in context.StudentAnswers
                   select new ModelStudentAnswer
                   {
                       SAID = studentanswer.SAID,
                       QuestionID = studentanswer.QuestionID,
                       Username = studentanswer.Username,
                       Answer = studentanswer.Answer,
                       Flagged = studentanswer.Flagged,
                       Type = studentanswer.Type,
                       StudentAnswerActive = studentanswer.StudentAnswerActive
                   };
        }

        public StudentAnswer ToEntity()
        {
            return new StudentAnswer
            {
                SAID = SAID,
                QuestionID = QuestionID,
                Username = Username,
                Answer = Answer,
                Flagged = Flagged,
                Type = Type,
                StudentAnswerActive = StudentAnswerActive
            };
        }

        public void Update(StudentAnswer studentanswer)
        {
            studentanswer.SAID = SAID;
            studentanswer.QuestionID = QuestionID;
            studentanswer.Username = Username;
            studentanswer.Answer = Answer;
            studentanswer.Flagged = Flagged;
            studentanswer.Type = Type;
            studentanswer.StudentAnswerActive = StudentAnswerActive;
        }
    }
}
