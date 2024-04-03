using Application.UseCases.AuthUseCases;
using Application.UseCases.Managers;
using Application.UseCases.UserUsecases;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class UserUseCaseManager : IUserUseCaseManager
    {
        private CreateUserUC _createUserUC;
        private DeleteUserUC _deleteUserUC;
        private GetByIdUserUC _getByIdUserUC;
        private GetAllUserUC _getAllUserUC;
        private UpdateUserUC _updateUserUC;
        private AuthenticationUC _authUserUC;
        public CreateUserUC CreateUserUC => _createUserUC;

        public DeleteUserUC DeleteUserUC => _deleteUserUC;

        public GetAllUserUC GetAllUserUC => _getAllUserUC;

        public GetByIdUserUC GetByIdUserUC => _getByIdUserUC;

        public UpdateUserUC UpdateUserUC => _updateUserUC;
        public AuthenticationUC AuthUserUC => _authUserUC;

        public UserUseCaseManager(CreateUserUC createUserUC, DeleteUserUC deleteUserUC,GetAllUserUC getAllUserUC, GetByIdUserUC getByIdUserUC,UpdateUserUC updateUserUC, AuthenticationUC authUserUC)
        {
            _createUserUC = createUserUC;
            _deleteUserUC = deleteUserUC;
            _getAllUserUC = getAllUserUC;
            _getByIdUserUC = getByIdUserUC;
            _updateUserUC = updateUserUC;
            _authUserUC = authUserUC;
        }
    }
}
