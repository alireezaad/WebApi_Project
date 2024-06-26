﻿using Application.Models.TaskEntityModels;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TaskEntityUseCases
{
    public class CreateTaskEntityUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public CreateTaskEntityUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }
        public async Task<TaskEntityGetModel> ExecuteAsync(TaskEntityPostModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Description) || model.UserId <= 0)
                {
                    throw new ArgumentException("Title and Description of Task is required!");
                }

                var taskEntity = _mapper.Map<TaskEntity>(model);
                var user = await _services.UserRepository.GetAsync(model.UserId);
                if (user == null)
                {
                    throw new ArgumentException("User has not found!");
                }
                taskEntity.AddUser(user);

                await _services.TaskEntityRepository.AddAsync(taskEntity);
                var viewModel = _mapper.Map<TaskEntityGetModel>(taskEntity);
                return viewModel;
                
            }
            catch
            { 
                throw;
            }
        }
    }
}
