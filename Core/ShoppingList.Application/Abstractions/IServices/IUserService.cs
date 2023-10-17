using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IServices
{
    public interface IUserService
    {
        List<User> GetAll();
        List<User> GetByActiveAll(bool status);
        User GetById(int Id);
        User GetByMail(string mail);
        (User, int kod, string message) Add(User user);
        bool Delete(User user);
        (User, int kod, string message) Update(User user);
    }
}
