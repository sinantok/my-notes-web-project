using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ViewModels
{
    public class RegisterViewModel
    {
        [DisplayName("User name"), Required(ErrorMessage = "{0} cannot be empty"), StringLength(30,ErrorMessage =
            "{0} max. {1} character.")]
        public string Username { get; set; }

        [DisplayName("E-Mail"), Required(ErrorMessage = "{0} connot be empty"), StringLength(80, ErrorMessage =
           "{0} max. {1} character."),
            EmailAddress(ErrorMessage = "{0} please enter a valid email address.")]
        public string Email { get; set; }

        [DisplayName("Password"), Required(ErrorMessage = "{0} cannot be empty"), DataType(DataType.Password), 
            StringLength(30, ErrorMessage ="{0} max. {1} character.")]
        public string Password { get; set; }

        [DisplayName("Re Password"), Required(ErrorMessage = "{0} connot be empty"), DataType(DataType.Password), 
            StringLength(30, ErrorMessage ="{0} max. {1} character."), 
            Compare("Password",ErrorMessage = "{1} and {0} do not match!")]
        public string RePassword { get; set; }
    }
}