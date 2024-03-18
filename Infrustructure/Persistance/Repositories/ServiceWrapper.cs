using Domain.IRepositories;
using Domain.Repositories;
using Infrustructure.Persistance.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class ServiceWrapper : IServiceWrapper
    {
        private IUserRepository _userRepository;
        private ITaskEntityRepository _taskEntityRepository;

        public IUserRepository UserRepository => _userRepository;

        public ITaskEntityRepository TaskEntityRepository => _taskEntityRepository;

        public ServiceWrapper(IUserRepository userRepository, ITaskEntityRepository taskEntityRepository)
        {
            _userRepository = userRepository;
            _taskEntityRepository = taskEntityRepository;
        }
    }
}
