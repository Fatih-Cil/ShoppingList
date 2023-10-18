using FluentValidation;
using ShoppingList.Application.ViewModels.ProductViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Validators.Products
{
    public class AddProductValidator: AbstractValidator<AddProductViewModel>
    {
        public AddProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün adı boş geçilemez")
                .NotNull().WithMessage("Ürün adı boş geçilemez")
                .Length(2, 30).WithMessage("Ürün adı en az 2 en çok 30 karakter olmalı");
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori id boş geçilemez")
                .NotNull().WithMessage("Kategori id boş geçilemez");
            RuleFor(x => x.UrlImage)
                .NotEmpty().WithMessage("Görsel link adresi boş geçilemez")
                .NotNull().WithMessage("Görsel link adresi boş geçilemez")
                .Length(2, 250).WithMessage("Ürün adı en az 2 en çok 250 karakter olmalı"); ;

        }
    }
}
