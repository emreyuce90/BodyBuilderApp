using BodyBuilder.Application.Dtos.Workout;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations.Workout {
    public class WorkoutAddDtoValidator :AbstractValidator<WorkoutAddDto>{
        public WorkoutAddDtoValidator()
        {
            RuleFor(w=>w.UserId).NotEmpty().WithMessage("UserId alanı boş geçilemez");
            RuleFor(w=>w.SubProgrammeId).NotEmpty().WithMessage("Subprogramme alanı boş geçilemez");
            RuleFor(w=>w.WorkoutDate).NotEmpty().WithMessage("Workout date alanı boş geçilemez");
            RuleFor(w=>w.StartTime).NotEmpty().WithMessage("Workout date alanı boş geçilemez");
        }
    }
}
