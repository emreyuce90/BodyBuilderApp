﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.WorkoutMovement {
    public class MovementSetDto {
        public int SetNumber { get; set; }
        public int Reps { get; set; }
        public float Weight { get; set; }
    }
}
