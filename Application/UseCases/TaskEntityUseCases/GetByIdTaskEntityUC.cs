using Application.Models.TaskEntityModels;
using AutoMapper;
using Domain.IRepositories;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.TaskEntityUseCases
{
    public class GetByIdTaskEntityUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public GetByIdTaskEntityUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        public async Task<TaskEntityGetModel> ExecuteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException();
                }
                var taskEntity = await _services.TaskEntityRepository.GetAsync(id);
                await _services.TaskEntityRepository.LoadCollections(taskEntity, t => t.Users);
                return _mapper.Map<TaskEntityGetModel>(taskEntity);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
