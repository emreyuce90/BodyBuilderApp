﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Workout {
    public class WorkoutDto {
        public Guid UserId { get; set; }
        public Guid SubProgrammeId { get; set; }
        public DateTime WorkoutDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
