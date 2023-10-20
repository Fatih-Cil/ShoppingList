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
    public class ListManager : IListService
    {
        IListRepository _listRepository;
        IUserService _userService;

        public ListManager(IListRepository listRepository, IUserService userService)
        {
            _listRepository = listRepository;
            _userService = userService;
        }

        public (List, int kod, string message) Add(List list)
        {
            User user = _userService.GetById(list.UserId);
            if (user == null) return (list, 0, "Bu id'ye ait bir kullanıcı bulunamadı");

            if (!_listRepository.Add(list))
            {
                return (list, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (list, 201, "Kayıt başarılı");

            }
        }

        public bool Delete(List list)
        {

            return _listRepository.Delete(list);
        }

        public List<List> GetAll()
        {
            throw new NotImplementedException();
        }

        public List GetById(int Id)
        {
            return _listRepository.Get(x => x.Id == Id);
        }

        public List GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<List> GetAllListByUserId(int userId)
        {
            return _listRepository.GetAll(x => x.UserId == userId);
        }

        public (List, int kod, string message) Update(List list)
        {
            User user = _userService.GetById(list.UserId);
            if (user == null) return (list, 0, "Bu id'ye ait bir kullanıcı bulunamadı");

            if (!_listRepository.Update(list))
            {
                return (list, 500, "Sunucu hatası! Güncelleme yapılamadı");
            }
            else
            {
                return (list, 201, "Kayıt başarılı");

            }
        }
    }
}
