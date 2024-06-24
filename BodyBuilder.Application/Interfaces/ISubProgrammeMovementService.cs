using BodyBuilder.Domain.Entities;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface ISubProgrammeMovementService {
        Task<Response> GetAllBySubProgrammeIdAsync(Guid subProgrammeId);
        Task<Response> UpdateSubProgrammeMovementByIdAsync(Guid subProgrammeId, List<SubProgrammeMovement> subProgrammeMovements);
    }
}
