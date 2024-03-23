using Application.UseCases.AuthUseCases;
using Application.UseCases.UserUsecases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Managers
{
    public interface IUserUseCaseManager
    {
        CreateUserUC CreateUserUC {  get; }
        DeleteUserUC DeleteUserUC { get; }
        GetAllUserUC GetAllUserUC { get; }
        GetByIdUserUC GetByIdUserUC { get; }
        UpdateUserUC UpdateUserUC { get; }
        AuthenticationUC AuthUserUC { get; }
    }
}
