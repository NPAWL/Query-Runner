using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryRunner.Models
{
    public class TestViewModel
    {
        public int QuestionID { get; private set; }
        public int QuestionNum { get; set; }
        public Boolean chekced { get; set; }
        public String QuestionText { get; set; }
        public String QuestionAnswer { get; set; }

        public TestViewModel(int questionID)
        {
            QuestionID = questionID;
        }

        public TestViewModel()
        {
        }
    }
}