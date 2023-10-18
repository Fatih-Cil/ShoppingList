using FluentValidation;
using ShoppingList.Application.ViewModels.CategoryViewModel;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.Categories
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryViewModel>
    {
        public AddCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş geçilemez")
                .NotNull().WithMessage("Kategori adı boş geçilemez")
                .Length(2, 30).WithMessage("Kategori adı en az 2 en çok 30 karakter olmalı");
        }
    }
}
