using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Domain.Entities;
using ShoppingList.Persistence.Concrate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Managers
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public (User, int kod, string message) Add(User user)
        {
            var result = _userRepository.Get(x => x.Email.ToUpper() == user.Email.ToUpper());

            if (result != null)
            {
                return (result, 0, "Bu mail adresi daha önce kullanılmış");
            }

            if (!_userRepository.Add(user))
            {
                return (user, 500, "Sunucu hatası! Kayıt yapılamadı");
            }
            else
            {
                return (user, 201, "Kayıt başarılı");
            }
        }

        public bool Delete(User user)
        {
            return _userRepository.Delete(user);
        }

        public List<User> GetAll()
        {
           return _userRepository.GetAll();
        }

        public List<User> GetByActiveAll(bool status)
        {
            return _userRepository.GetAll(x=>x.Status == status);
        }

        public User GetById(int Id)
        {
            return _userRepository.Get(x => x.Id == Id);
        }

        public User GetByMail(string mail)
        {
          return  _userRepository.Get(x=>x.Email == mail);
           
        }

        public (User, int kod, string message) Update(User user)
        {
            User result = GetByMail(user.Email);
            if (result != null && result.Id!=user.Id) 
            { 
                return (user, 0, "Bu mail adresi başka bir kullanıcı tarafından kullanılıyor"); 
            }

            if (!_userRepository.Update(user))
            {
                return (user, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (user, 200, "Güncelleme başarılı");

            }
        }
    }
}
