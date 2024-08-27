using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class SubProgrammeMovement :BaseEntity{
        public SubProgrammeMovement()
        {
            Movement = new Movement();
        }
        public Guid MovementId { get; set; }
        public Guid SubProgrammeId { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }

        public Movement? Movement { get; set; }
    }
}
