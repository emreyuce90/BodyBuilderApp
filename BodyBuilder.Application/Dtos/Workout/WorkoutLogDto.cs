using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Workout {
    public class WorkoutLogDto {
        public DateTime WorkoutDate{ get; set; }
        public int WorkoutTime { get; set; }
        public string WorkoutName { get; set; }

    }
}
