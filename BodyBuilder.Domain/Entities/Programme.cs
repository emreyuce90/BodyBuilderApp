using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class Programme :BaseEntity{

        [StringLength(100, ErrorMessage = "Title alanı 100 karakterden fazla olamaz")]  
        public required string Name { get; set; }
        public Guid UserId { get; set; }
        public virtual List<SubProgramme> SubProgrammes { get; set; }
    }
}
