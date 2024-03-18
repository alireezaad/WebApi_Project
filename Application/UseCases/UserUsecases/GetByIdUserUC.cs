using Application.Models.UserModels;
using AutoMapper;
using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UserUsecases
{
    public class GetByIdUserUC
    {
        private readonly IServiceWrapper _services;
        private readonly IMapper _mapper;
        public GetByIdUserUC(IServiceWrapper services, IMapper mapper)
        {
            _services = services;
            _mapper = mapper;
        }

        public async Task<UserGetModel> ExecuteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid Id");
                }

                var user = await _services.UserRepository.GetAsync(id);
                await _services.UserRepository.LoadCollections(user, u => u.Tasks);
                return _mapper.Map<UserGetModel>(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
