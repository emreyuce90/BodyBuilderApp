using BodyBuilder.Application.Dtos.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuilder.Application.Validations.Auth {
    public class UserAddDtoValidator:AbstractValidator<UserAddDto> {
        public UserAddDtoValidator()
        {
            //RuleFor(u => u.Name)
            //     .NotNull()
            //     .NotEmpty()
            //     .WithMessage("İsim alanı boş geçilemez");
            //RuleFor(u => u.Surname)
            //     .NotNull()
            //     .NotEmpty()
            //     .WithMessage("Soyisim alanı boş geçilemez");

            RuleFor(u => u.Email)
                 .NotNull()
                 .NotEmpty()
                 .WithMessage("E-mail adresi boş geçilemez")
                 .EmailAddress()
                 .WithMessage("E-Mail adresi geçerli değil");


            RuleFor(u => u.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("Şifre alanı boş geçilemez")
                .MinimumLength(6).WithMessage("Parola en az 6 karakter olmalıdır.")
                .Matches("[0-9]").WithMessage("Parola en az bir rakam içermelidir.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Parola en az bir özel karakter içermelidir.");
                ;

            RuleFor(u => u.PasswordConfirm)
                .NotNull()
                .NotEmpty()
                .WithMessage("Şifre tekrar alanı boş geçilemez")
                .Equal(x => x.Password)
                .WithMessage("Şifre ve şifre tekrar alanı aynı olmalıdır");
        }
    }
}
