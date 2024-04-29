using BodyBuilder.Application.Dtos.Bodypart;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations.BodyPart {
    public class BodyPartAddValidator:AbstractValidator<BodyPartAddDto> {
        public BodyPartAddValidator()
        {
            RuleFor(x=>x.Name).NotEmpty().WithMessage("Name alanı boş geçilemez");
        }
    }
}
