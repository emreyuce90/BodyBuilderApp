using BodyBuilder.Application.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.ValidationRules.User {
    public class UserAddDtoValidator:AbstractValidator<UserAddDto> {
        public UserAddDtoValidator()
        {
            RuleFor(u => u.Email)
                 .NotNull()
                 .NotEmpty()
                 .WithMessage("E-mail adresi boş geçilemez")
                 .EmailAddress()
                 .WithMessage("E-Mail adresi geçerli değil");
                 

            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Şifre alanı boş geçilemez");

            RuleFor(u => u.PasswordConfirm)
                .NotNull()
                .NotEmpty()
                .WithMessage("Şifre tekrar alanı boş geçilemez")
                .Equal(x => x.Password)
                .WithMessage("Şifre ve şifre tekrar alanı aynı olmalıdır");

            RuleFor(u => u.PhoneNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("Telefon numarası alanı boş geçilemez");

            RuleFor(u => u.Gender)
                .NotEmpty()
                .WithMessage("Cinsiyet alanı boş geçilemez");

            RuleFor(u => u.DateOfBirth)
                .NotNull()
                .NotEmpty()
                .WithMessage("Doğum günü alanı boş geçilemez");
        }
    }
}
