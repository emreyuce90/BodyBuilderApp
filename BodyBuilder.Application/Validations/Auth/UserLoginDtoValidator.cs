using BodyBuilderApp.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations {
    public class UserLoginDtoValidator:AbstractValidator<UserLoginDto> {
        public UserLoginDtoValidator()
        {
            RuleFor(u => u.Email)
                .NotNull()
                .WithMessage("E-Mail alanı boş geçilemez")
                .EmailAddress()
                .WithMessage("Girdiğiniz format hatalı,lütfen doğru bir e-mail formatı giriniz");
            RuleFor(u => u.Password)
                .NotNull()
                .WithMessage("Password alanı boş geçilemez");
        }
    }
}
