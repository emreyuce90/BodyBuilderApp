using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class SubProgrammeMovement :BaseEntity{
        [ForeignKey("Movement")]
        public Guid MovementId { get; set; }
        public Guid SubProgrammeId { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public virtual SubProgramme SubProgramme { get; set; }
        public virtual Movement? Movement { get; set; }
    }
}
