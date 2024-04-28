using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class SubProgramme :BaseEntity{

        public Guid ProgrammeId { get; set; }
        [StringLength(100, ErrorMessage = "Title alanı 100 karakterden fazla olamaz")]

        public required string Name { get; set; }
        //public virtual List<SubProgrammeMovement> SubProgrammeMovements{ get; set; }
        //[Range(0,int.MaxValue)]
        //public int Sets { get; set; }
        //[Range(0, int.MaxValue)]
        //public int Reps { get; set; }
    }
}
