using Application.Models.UserModels;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UserUsecases
{
    public class UpdateUserUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public UpdateUserUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper= mapper;
        }

        public async Task<UserGetModel> ExecuteAsync(int id, UserPutModel model)
        {
            try
            {
                if (id <= 0)
                {

                }
                if (model == null)
                {
                    throw new ArgumentNullException("Model is null!");
                }
                var user = await _services.UserRepository.GetAsync(id);
                // in RESTful if an entity was not avaliable for modifying we should create and return in to user:
                if (user == null)
                {
                    user = _mapper.Map<User>(model);
                    var addedUser = await _services.UserRepository.AddAsync(user);
                    return _mapper.Map<UserGetModel>(addedUser);
                }
                user.Email = model.Email;
                user.Password = model.Password;
                var modifiedUser = await _services.UserRepository.UpdateAsync(user);
                return _mapper.Map<UserGetModel>(modifiedUser);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
