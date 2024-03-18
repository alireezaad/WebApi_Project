using Application.Models.UserModels;
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
        public async Task<bool> Authenticate(UserAuthorizeModel model)
        {
            if (model == null) throw new ArgumentNullException("Email or password is null!");
            var user = await _authUser.FindByEmail(model.Email);
            return _authUser.VerifyPassword(user, model.Password);
        }
    }
}
