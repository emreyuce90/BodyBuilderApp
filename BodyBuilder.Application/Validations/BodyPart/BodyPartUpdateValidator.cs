using BodyBuilder.Application.Dtos.Bodypart;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations.BodyPart {
    public class BodyPartUpdateValidator:AbstractValidator<BodyPartUpdateDto> {
        public BodyPartUpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name alanı boş geçilemez");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name alanı en fazla 100 karakter olabilir");
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id alanı boş geçilemez");
        }
    }
}
