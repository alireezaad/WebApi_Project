using Domain.Entities;
using Domain.IRepositories;
using Infrustructure.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class AuthUser : IAuthUser
    {
        private readonly ApiDbContext _dbContext;
        public AuthUser(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> FindByEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException("Email is Null!");
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            if(user == null)
            {
                throw new ArgumentException("Email is invalid");
            }
            return user;
        }

        public bool VerifyPassword(User user, string password)
        {
            return user != null && user.Password == password;
        }
    }
}
