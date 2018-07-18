using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IQuestion
    {
        IQueryable<ModelQuestion> ReadQuestions();
        ModelQuestion GetQuestion(int qid);
        IQueryable<ModelQuestion> GetQuestionsByTest(int testid);
        void CreateQuestion(ModelQuestion model);
        void CreateQuestion(ModelQuestion model, int testid);
        void DeleteQuestion(ModelQuestion model);
        void UpdateQuestion(ModelQuestion model);
    }
}
