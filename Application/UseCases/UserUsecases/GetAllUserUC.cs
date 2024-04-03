using Application.Models.UserModels;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
 

namespace Application.UseCases.UserUsecases
{
    public class GetAllUserUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public GetAllUserUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }
        public async Task<List<UserGetModel>> ExecuteAsync(params Expression<Func<User, object>>[]? expression)
        {
            var users = await _services.UserRepository.GetAllAsync(expression);
            var usersViewModels = _mapper.Map<List<UserGetModel>>(users);
            return usersViewModels;
        }
    }
}
