using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QueryRunner.Models
{
    public class DisplayStudentTestsModel
    {
        [Required]
        public int TestID;
        [Display(Name = "Test")]
        public string TestName;
        [Display(Name = "Test Date")]
        public DateTime Date;
        [Display(Name = "Mark")]
        public string Mark;

        public DisplayStudentTestsModel(int testID)
        {
            TestID = testID;
        }
    }
}