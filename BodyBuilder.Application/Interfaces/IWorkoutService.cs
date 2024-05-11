using BodyBuilder.Application.Dtos.Workout;
using BodyBuilderApp.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Interfaces {
    public interface IWorkoutService {
        Task<Response> StartWorkoutAsync(WorkoutAddDto workoutAddDto);
        Task<Response> RemoveWorkoutAsync(Guid workoutId);
        Task<Response> GetWorkoutBySubprogrammeIdAsync(Guid subProgrammeId);
        Task<Response> GetWorkoutByIdAsync(Guid workoutId);
        Task<Response> GetWorkoutByUserIdAsync(Guid userId);
        Task<Response> FinishWorkout(Guid workoutId,TimeSpan endTime);

    }
}
