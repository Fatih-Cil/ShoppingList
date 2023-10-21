using FluentValidation;
using ShoppingList.Application.ViewModels.ProductListViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.ProductLists
{
    public class UpdateProductListValidator: AbstractValidator<UpdateProductListModel>
    {
        public UpdateProductListValidator()
        {
            RuleFor(x => x.Description)
                     .MaximumLength(200).WithMessage("En çok 200 karakter olmalı");
        }
    }
}
