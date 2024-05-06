using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.Programme {
    public class ProgrammeAddDto {
        public string Name { get; set; }
        public Guid UserId { get; set; }
    }
}
