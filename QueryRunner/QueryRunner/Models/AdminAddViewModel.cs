using System;
using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Library.Models;

namespace QueryRunner.Models
  {
  public class AdminAddViewModel
    {
      private ModelUser _model = new ModelUser();

      [Required]
      public string Usernam { get { return _model.Username; } set { _model.Username = value; } } 
      [Required]
      public string Password { get { return _model.PasswordHash; } set { _model.PasswordHash = value; } }
      [Compare("Password", ErrorMessage = "Confirm password does not match!")]
      [DataType(DataType.Password)]
      public string ConPassword { get; set; }
      public bool UserActive { get { return _model.UserActive; } set { _model.UserActive = value; } } 

      public AdminAddViewModel() {}
      public AdminAddViewModel(ModelUser model) { _model = model; }
      public ModelUser ToDataModel() { return _model; }

    }
  }