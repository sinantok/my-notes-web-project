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
    [Table("Categories")]
    public class ECategory : MyEntityBase
    {
        [DisplayName("Category"),Required,StringLength(50)]
        public string Title { get; set; }

        [DisplayName("Description"),StringLength(150)]
        public string Description { get; set; }

        public virtual List<ENote> Notes { get; set; }

        public ECategory()
        {
            Notes = new List<ENote>();
        }
  
    }
}
