using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelTest
    {
        public int TestID { get; set; }
        public string TestName { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool TestActive { get; set; }

        internal IQueryable<ModelTest> Get(QueryRunnerEntities context)
        {
            return from test in context.Tests
                   select new ModelTest
                   {
                       TestID = test.TestID,
                       TestName = test.TestName,
                       Username = test.Username,
                       Date = test.Date,
                       StartTime = test.StartTime,
                       EndTime = test.EndTime,
                       TestActive = test.TestActive
                   };
        }

        public Test ToEntity()
        {
            return new Test
            {
                TestID = TestID,
                TestName = TestName,
                Username = Username,
                Date = Date,
                StartTime = StartTime,
                EndTime = EndTime,
                TestActive = TestActive
            };
        }

        public void Update(Test test)
        {
            test.TestID = TestID;
            test.TestName = TestName;
            test.Username = Username;
            test.Date = Date;
            test.StartTime = StartTime;
            test.EndTime = EndTime;
            test.TestActive = TestActive;
        }
    }
}
