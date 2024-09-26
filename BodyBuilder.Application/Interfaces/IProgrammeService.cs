using BodyBuilder.Application.Dtos.Programme;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IProgrammeService {
        Task<Response> GetAllAsync();
        Task<Response> GetByIdAsync(Guid programmeId);
        Task<Response> GetByUserIdAsync(Guid userId);
        Task<Response> AddAsync(ProgrammeAddDto programmeAddDto);
        Task<Response> CreateCustomWorkoutAsync(Guid userId, CreateCustomProgramme customProgramme);
        Task<Response> DeleteProgrammeAsync(Guid programmeId);

    }
}
