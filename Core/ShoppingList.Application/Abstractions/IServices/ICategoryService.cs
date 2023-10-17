using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IServices
{
    public interface ICategoryService
    {
        List<Category> GetAll();
        Category GetById(int Id);
        Category GetByName(string Name);
        (Category ,int kod,string message) Add(Category category);
        bool Delete(Category category);
        (Category, int kod, string message) Update(Category category);
        
    }
}
