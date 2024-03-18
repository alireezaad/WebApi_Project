using Application.Models.TaskEntityModels;
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
    public class GetAllTaskEntityUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public GetAllTaskEntityUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }
        public async Task<List<TaskEntityGetModel>> ExecuteAsync()
        {
            // with loading of all users
            //var taskEntities = await _services.TaskEntityRepository.GetAllAsync(t => t.Users);
            var taskEntities = await _services.TaskEntityRepository.GetAllAsync();
            var taskViewModels = _mapper.Map<List<TaskEntityGetModel>>(taskEntities);
            return taskViewModels;
        }
    }
}
