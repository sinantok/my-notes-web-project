using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EUser : MyEntityBase
    {
        [DisplayName("Name"), StringLength(30)]
        public string Name { get; set; }

        [DisplayName("Surname"), StringLength(30)]
        public string Surname { get; set; }

        [DisplayName("User name"), Required(ErrorMessage = "{0} cannot be empty"),StringLength(30)]
        public string Username { get; set; }

        [DisplayName("E-Mail"), Required(ErrorMessage = "{0} cannot be empty"),StringLength(80)]
        public string Email { get; set; }

        [DisplayName("Passwprd"), Required(ErrorMessage = "{0} cannot be empty"),StringLength(30)]
        public string Password { get; set; }

        [Required]
        public Guid ActiveGuid { get; set; }

        [StringLength(100)]  // /images/user_13.jpeg 
        public string ProfileImageFileName { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }

        public virtual List<ENote> Notes { get; set; }
        public virtual List<EComment> Comments { get; set; }
        public virtual List<ELiked> Likes { get; set; }
    }
}
