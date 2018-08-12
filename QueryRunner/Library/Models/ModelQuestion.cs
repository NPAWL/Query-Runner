using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelQuestion
    {
        public int QuestionID { get; set; }
        public int TestID { get; set; }
        public string Instruction { get; set; }
        public int MaxMark { get; set; }
        public bool QuestionActive { get; set; }
        public string QuestionAnswer { get; set; }

        internal IQueryable<ModelQuestion> Get(QueryRunnerEntities context)
        {
            return from question in context.Questions
                   select new ModelQuestion
                   {
                       //QuestionID = question.QuestionID,
                       TestID = question.TestID,
                       Instruction = question.Instruction,
                       QuestionActive = question.QuestionActive,
                       QuestionAnswer = question.QuestionAnswer
                   };
        }

        public Question ToEntity()
        {
            return new Question
            {
                //QuestionID = QuestionID,
                TestID = TestID,
                Instruction = Instruction,
                QuestionActive = QuestionActive,
                QuestionAnswer = QuestionAnswer,
                MaxMark = MaxMark
            };
        }

        public void Update(Question question)
        {
            //question.QuestionID = QuestionID;
            question.TestID = TestID;
            question.Instruction = Instruction;
            question.QuestionActive = QuestionActive;
            question.QuestionAnswer = QuestionAnswer;
            question.MaxMark = MaxMark;
        }
    }
}
