using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryRunner.Models
{
    public class TestViewModel
    {
        public int QuestionID;
        public int QuestionNum;
        public Boolean flagged;
        public String QuestionText;
        public String QuestionAnswer;

        public TestViewModel(int questionID)
        {
            QuestionID = questionID;
        }

        public TestViewModel Get()
        {
            return this;
        }
    }
}