using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Library.Models;
using QueryRunner.Helpers;

namespace QueryRunner.Models
  {
  public class CreateTestViewModel
    {
      private ModelTest _model = new ModelTest();

      [Required]
      public string TestName { get { return _model.TestName; } set { _model.TestName = value; } }
      [Required]
      public DateTime TestDate { get { return _model.Date; } set { _model.Date = value; } }
      [Required]
      public DateTime TestStartTime { get { return _model.StartTime; } set { _model.StartTime = value; } }
      [Required]
      public DateTime TestEndTime { get { return _model.EndTime; } set { _model.EndTime = value; } }
      public List<CreateQuestionViewModel> Questions { get; set; }
      [Required]
      public int NumberOfQuestions { get; set; }
      public int QuestionNumber { get; set; }

      public CreateTestViewModel() {}
      public CreateTestViewModel(ModelTest model) { _model = model; }
      public ModelTest ToDataModel() { return _model; }
      public List<ModelQuestion> QuestionsToDataModel()
        {
          List<ModelQuestion> final = new List<ModelQuestion>();
          foreach (CreateQuestionViewModel item in Questions)
            final.Add(item.ToDataModel());
          return final;
        }

    }
  }