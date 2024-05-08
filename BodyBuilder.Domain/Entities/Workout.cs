using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class Workout :BaseEntity{
        public Guid UserId { get; set; }
        public Guid SubProgrammeId { get; set; }
        public DateTime WorkoutDate { get;set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SubProgramme SubProgramme { get; set; }
    }
}
