using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QueryRunner.Models
{
    public class ViewStudentMarks
    {

        public ViewStudentMarks(int testID, string key, double v)
        {
            this.TestID = testID;
            this.Name = key;
            this.Presentage = v;
        }

        [Required]
        public int TestID { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public double Presentage { get; set; }
    }
}