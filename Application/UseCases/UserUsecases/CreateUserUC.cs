using Application.Models.UserModels;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UserUsecases
{
    public class CreateUserUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public CreateUserUC(IServiceWrapper services,IMapper mapper)
        {
            _mapper = mapper;
            _services = services;
        }
        public async Task<UserGetModel> ExecuteAsync(UserPostModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                {
                    throw new ArgumentException("Email and password are required!");
                }
                if (model == null)
                {
                    throw new ArgumentNullException("User is null!");
                }

                var user = _mapper.Map<UserPostModel,User>(model);

                var taskEntity = new TaskEntity();
                if (model.TaskEntityId > 0)
                {
                    taskEntity = await _services.TaskEntityRepository.GetAsync(model.TaskEntityId); 
                    user.AddTaskEntity(taskEntity);
                }

                //await _userRepository.LoadTasksAsync(user);
                await _services.UserRepository.AddAsync(user);
                var viewModel = _mapper.Map<UserGetModel>(user);
                //viewModel.Tasks = user.Tasks.Select(u => u.Id);
                return viewModel;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
    
}
