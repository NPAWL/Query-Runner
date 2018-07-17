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
        void CreateStudentAnswer(ModelStudentAnswer model);
        void DeleteStudentAnswer(ModelStudentAnswer model);
        void UpdateStudentAnswer(ModelStudentAnswer model);
    }
}
