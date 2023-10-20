using FluentValidation;
using ShoppingList.Application.ViewModels.ListViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.Lists
{
    public class AddListValidator: AbstractValidator<AddListViewModel>
    {
        public AddListValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Liste adı boş geçilemez")
               .NotNull().WithMessage("Liste adı boş geçilemez")
               .Length(2, 30).WithMessage("Liste adı en az 2 en çok 30 karakter olmalı");
        }
    }
}
