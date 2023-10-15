using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Application.Abstractions.IRepositories
{
    public interface IEntityRepository<T> where T : class, new()
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null); //filtre önemli değil
        T Get(Expression<Func<T, bool>> filter); //filtre önemli
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
