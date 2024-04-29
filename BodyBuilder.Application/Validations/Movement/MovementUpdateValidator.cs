using BodyBuilder.Application.Dtos.Movement;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations.Movement {
    public class MovementUpdateValidator:AbstractValidator<MovementUpdateDto> {
        public MovementUpdateValidator()
        {
            RuleFor(x=>x.Id).NotEmpty().WithMessage("Id alanı boş geçilemez");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title alanı boş geçilemez");
            RuleFor(x => x.Title).MaximumLength(100).WithMessage("Title alanı en fazla 100 karakter olabilir");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description alanı boş geçilemez");
        }
    }
}
