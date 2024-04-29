using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Bodypart {
    public class BodyPartUpdateDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
