using FluentValidation;
using ShoppingList.Application.ViewModels.AuthViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.Logins
{
    public class LoginValidator : AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş geçilemez")
                .NotNull().WithMessage("Email boş geçilemez")
                .EmailAddress().WithMessage("Bu bir mail adresi değildir");

            RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Şifre boş geçilemez")
            .Length(8, 20).WithMessage("En az 8, en çok 20 karakter girilmeli")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$").WithMessage("Şifre Büyük harf, küçük harf ve rakamdan oluşmalı");
        }
    }
}
