﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Domain.Entities {
    public class SubBodyPart :BaseEntity{
        public string Name { get; set; }= string.Empty;
        public Guid BodyPartId { get; set; }

    }
}
