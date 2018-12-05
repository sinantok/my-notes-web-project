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
    [Table("Notes")]
    public class ENote : MyEntityBase
    {
        [DisplayName("Note Title"),Required,StringLength(60)]
        public string Title { get; set; }

        [DisplayName("Text"), Required, StringLength(5000)]
        public string Text { get; set; }

        [DisplayName("Draft")]
        public bool IsDraft { get; set; }

        [DisplayName("Like Count")]
        public int LikeCount { get; set; }

        public virtual int CategoryId { get; set; } //table for which it is associated

        public virtual EUser Owner { get; set; }
        public virtual ECategory Category { get; set; }
        public virtual List<EComment> Comments { get; set; }
        public virtual List<ELiked> Likes { get; set; }

        public ENote()
        {
            Comments = new List<EComment>();
            Likes = new List<ELiked>();
        }
    }
}
