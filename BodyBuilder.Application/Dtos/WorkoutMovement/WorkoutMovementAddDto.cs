using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Dtos.WorkoutMovement {
    public class WorkoutMovementAddDto {
        public Guid MovementId { get; set; }
        public List<MovementSetDto> MovementSetDtos { get; set; }
    }
}
