using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class Movement : BaseEntity {
        [StringLength(100, ErrorMessage = "Title alanı 100 karakterden fazla olamaz")]
        public  string Title { get; set; }

        public  string Description { get; set; }

        public string Tip { get; set; }
        public  string ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public Guid BodyPartId { get; set; }
        public virtual List<SubProgrammeMovement> SubProgrammeMovements { get; set; }
    }
}
