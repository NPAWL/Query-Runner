using System;
using System.Collections.Generic;   
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Library.Models;

namespace QueryRunner.Models
  {
  public class AddStudentsToTestModel
    {
      [Required]
      public int TestID { get; set; }
      [Required]
      public HttpPostedFileBase file { get; set; }

      public AddStudentsToTestModel() {}
      public AddStudentsToTestModel(int TestId) { TestID = TestId; }
    }
  }