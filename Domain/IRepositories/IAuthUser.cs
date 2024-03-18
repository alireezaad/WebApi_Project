using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IAuthUser
    {
        Task<User> FindByEmail(string email);
        bool VerifyPassword(User user, string password);
    }
}
