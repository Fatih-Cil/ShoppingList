using FluentValidation;
using ShoppingList.Application.ViewModels.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.Users
{
    public class AddUserValidator : AbstractValidator<AddUserViewModel>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("İsim Boş geçilemez")
                .NotNull().WithMessage("İsim Boş geçilemez")
                .Length(2, 30).WithMessage("En az 2 en çok 30 karakter olmalı");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("İsim Boş geçilemez")
                .NotNull().WithMessage("İsim Boş geçilemez")
                .Length(2, 30).WithMessage("En az 2 en çok 30 karakter olmalı");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş geçilemez")
                .NotNull().WithMessage("Email boş geçilemez")
                .EmailAddress().WithMessage("Bu bir mail adresi değildir");

            RuleFor(a => a.Password)
            .NotEmpty().WithMessage("Şifre boş geçilemez")
            .Length(8, 20).WithMessage("En az 8, en çok 20 karakter girilmeli")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$").WithMessage("Şifre Büyük harf, küçük harf ve rakamdan oluşmalı");

            RuleFor(a => a.PasswordRetry).Equal(a => a.Password).WithMessage("Şifre eşleşmiyor");

        }
    }
}
