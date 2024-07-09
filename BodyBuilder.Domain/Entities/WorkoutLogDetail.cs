using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class WorkoutLogDetail {
        public Guid WorkoutId { get; set; }
        public string WorkoutName { get; set; }=string.Empty;
        public DateTime WorkoutDate { get; set; }
        public TimeSpan WorkoutTime { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SetNumber { get; set; }
        public float Weight { get; set; }
        public int Reps { get; set; }
    }
}
