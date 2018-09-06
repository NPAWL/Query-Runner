using System;
using System.Collections.Generic;    
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;    
using Library.Models;
using QueryRunner.Helpers;

namespace QueryRunner.Models
  {
  public class DeleteTestViewModel
    {
      private ModelTest _model = new ModelTest();

      [Required]
      public int TestID { get { return _model.TestID; } set { _model.TestID = value; } }
      [Required]
      public string TestName { get { return _model.TestName; } set { _model.TestName = value; } }
      [Required]
      public string TestUsername { get { return _model.Username; } set { _model.Username = value; } }
      [Required]
      public DateTime TestDate { get { return _model.Date; } set { _model.Date = value; } }
      [Required]
      public DateTime TestStartTime { get { return _model.StartTime; } set { _model.StartTime = value; } }
      [Required]
      public DateTime TestEndTime { get { return _model.EndTime; } set { _model.EndTime = value; } }   
      [Required]
      public bool TestActive { get { return _model.TestActive; } set { _model.TestActive = value; } }

      public DeleteTestViewModel() {}
      public DeleteTestViewModel(ModelTest model) { _model = model; }
      public ModelTest ToDataModel() { return _model; }

    }
  }