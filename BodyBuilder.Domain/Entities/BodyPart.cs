using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BodyBuilder.Domain.Entities {
    public class BodyPart : BaseEntity {
        [StringLength(100, ErrorMessage = "100 karakterden fazla olamaz")]
        public  string Name{ get; set; }
        public string PictureUrl { get; set; }
        public virtual List<Movement> Movements { get; set; }
    }
}
