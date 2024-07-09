using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class WorkoutMovementSet:BaseEntity {
        public Guid WorkoutMovementId { get; set; }
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
    }
}
