using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IServices
{
    public interface IListService
    {
        List<List> GetAll();
        List GetById(int Id);
        List GetByName(string name);
        List<List> GetAllListByUserId(int userId);
        (List, int kod, string message) Add(List list);
        bool Delete(List list);
        (List, int kod, string message) Update(List list);
    }
}
