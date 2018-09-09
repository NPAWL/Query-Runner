using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Library.Models;

namespace QueryRunner.Models
  {
  public class CreateQuestionViewModel
    {
      private ModelQuestion _model = new ModelQuestion();

      [Required]
      public string QuestionInstruction { get { return _model.Instruction; } set { _model.Instruction = value; } }
      [Required]
      public int QuestionMaxMark { get { return _model.MaxMark; } set { _model.MaxMark = value; } }
      [Required]
      public string QuestionAnswer { get { return _model.QuestionAnswer; } set { _model.QuestionAnswer = value; } }
      public int QuestionNum { get; set; }

      public CreateQuestionViewModel() {}
      public CreateQuestionViewModel(int i) { QuestionNum = i; }
      public CreateQuestionViewModel(ModelQuestion model) { _model = model; }
      public ModelQuestion ToDataModel() { return _model; }

      public bool IsValid()
      {
        if (QuestionInstruction == "" || QuestionAnswer == "")
          return false;
        if (QuestionMaxMark < 1)
          return false;
        return true;
      }

    }
  }