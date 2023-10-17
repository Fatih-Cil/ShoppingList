using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Managers
{
    public class CategoryManager : ICategoryService
    {

        ICategoryRepository _categoryRepository;
       
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            
        }




        public (Category, int kod, string message) Add(Category category)
        {

            var result = _categoryRepository.Get(x => x.Name.ToUpper() == category.Name.ToUpper());

            if (result != null)
            {
                return (result, 0, "Bu isimde bir kategori var");
            }

            if (!_categoryRepository.Add(category))
            {
                return (category, 500, "Sunucu hatası! Kayıt yapılamadı");
            }
            else
            {
                return (category, 201, "Kayıt başarılı");
            }



        }

        public bool Delete(Category category)
        {
            return _categoryRepository.Delete(category);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public (Category, int kod, string message) Update(Category category)
        {
            var result = _categoryRepository.Get(x => x.Name.ToUpper() == category.Name.ToUpper());

            if (result != null)
            {
                return (result, 0, "Bu isimde bir kategori var");
            }

            if (!_categoryRepository.Update(category))
            {
                return (category, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (category, 200, "Güncelleme başarılı");

            }
        }

        public Category GetById(int Id)
        {
            return _categoryRepository.Get(x => x.Id == Id);
        }



        public Category GetByName(string name)
        {
            return _categoryRepository.Get(x => x.Name.ToUpper() == name.ToUpper());

        }


    }
}
