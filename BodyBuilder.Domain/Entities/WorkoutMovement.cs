using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class WorkoutMovement:BaseEntity {
        public Guid WorkoutId { get; set; }
        public Guid MovementId { get; set; }
        public Workout Workout { get; set; }
        public Movement Movement { get; set; }
        public List<WorkoutMovementSet> WorkoutMovementSets { get; set; }
    }
}
