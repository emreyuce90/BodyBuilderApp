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

        public  string Name { get; set; }
        public virtual List<SubProgrammeMovement> SubProgrammeMovements{ get; set; }
        public virtual List<Workout> Workouts { get; set; }
   
    }
}
