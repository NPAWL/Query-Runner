using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IStudentAnswer
    {
        IQueryable<ModelStudentAnswer> ReadStudentAnswers();
        ModelStudentAnswer GetStudentAnswer(int said);
        IQueryable<ModelStudentAnswer> GetStudentAnswersByStudent(string username);
        IQueryable<ModelStudentAnswer> GetStudentAnswersByTest(int testid);
        IQueryable<ModelStudentAnswer> GetStudentAnswersByQuestion(int qid);
        IQueryable<ModelStudentAnswer> GetStudentAnswersByStudentByTest(string username, int testid);
        void CreateStudentAnswer(ModelStudentAnswer model);
        void CreateStudentAnswer(ModelStudentAnswer model, string username, int qid);
        void DeleteStudentAnswer(ModelStudentAnswer model);
        void UpdateStudentAnswer(ModelStudentAnswer model);
    }
}
