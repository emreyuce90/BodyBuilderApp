﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class SubProgrammeMovement :BaseEntity{
        public Guid MovementId { get; set; }
        public Guid SubProgrammeId { get; set; }
    }
}