using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.SubProgramme {
    public class SubProgrammeDto {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProgrammeId { get; set; }
    }
}
