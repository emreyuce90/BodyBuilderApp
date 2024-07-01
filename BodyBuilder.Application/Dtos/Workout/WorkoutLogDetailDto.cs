using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class WorkoutLogDetailDto {
        public WorkoutLogDetailDto()
        {
            MovementDetails = new List<MovementDetail>();
        }
        public Guid WorkoutId { get; set; }
        public string WorkoutName { get; set; }=string.Empty;
        public DateTime WorkoutDate { get; set; }
        public TimeSpan WorkoutTime { get; set; }
        public int Duration { get; set; }
        public List<MovementDetail> MovementDetails{ get; set; }
    }
}
