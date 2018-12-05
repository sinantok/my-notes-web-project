using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEvernote.Entities.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("User name"), Required(ErrorMessage = "{0} cannot be empty"), StringLength(30, ErrorMessage =
            "{0} max. {1} character.")]
        public string Username { get; set; }

        [DisplayName("Password"), Required(ErrorMessage = "{0} cannot be empty"), DataType(DataType.Password), StringLength(30, ErrorMessage =
             "{0} max. {1} character.")]
        public string Password { get; set; }
    }
}