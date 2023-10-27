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
    public class AuthManager : IAuthService
    {
        IUserRepository _userRepository;

        public AuthManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User LoginCheck(User user)
        {
            User resultUser = _userRepository.Get(x => x.Email == user.Email && x.Status == true);
            if (resultUser == null) { return resultUser; }
            if (string.Equals(user.Password, resultUser.Password, StringComparison.Ordinal))
            {
                return resultUser;
            }
            resultUser = null;
            return resultUser;
        }
    }
}
