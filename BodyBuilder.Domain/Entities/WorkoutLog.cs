using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class WorkoutLog {
        public Guid WorkoutId { get; set; }
        public DateTime WorkoutDate { get; set; }
        public int WorkoutTime { get; set; }
        public string WorkoutName { get; set; }
    }
}
