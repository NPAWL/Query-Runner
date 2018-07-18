using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface ITest
    {
        IQueryable<ModelTest> ReadTests();
        ModelTest GetTest(int testid);
        ModelTest GetTestByName(string testname);
        void CreateTest(ModelTest model);
        void CreateTest(ModelTest model, string username, List<ModelQuestion> questions);
        void DeleteTest(ModelTest model);
        void UpdateTest(ModelTest model);
    }
}
