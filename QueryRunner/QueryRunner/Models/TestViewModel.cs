using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryRunner.Models
{
    public class TestViewModel
    {
        public int QuestionID;
        public Boolean chekced;
        public String QuestionText;
        public String QuestionAnswer;

        public TestViewModel(int questionID)
        {
            QuestionID = questionID;
        }
    }
}