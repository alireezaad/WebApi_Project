using Application.Models.UserModels;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.AuthUseCases
{
    public class AuthUserUC
    {
        private readonly IAuthUser _authUser;
        public AuthUserUC(IAuthUser authUser)
        {
            _authUser = authUser;
        }
        public async Task<Tuple<User,bool>> Authenticate(UserAuthorizeModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password)) throw new ArgumentNullException("Email or password is null!");
                var user = await _authUser.FindByEmail(model.Email);
                if (_authUser.VerifyPassword(user, model.Password))
                    return new Tuple<User, bool>(user, true);
                else
                    return new Tuple<User, bool>(null, false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
