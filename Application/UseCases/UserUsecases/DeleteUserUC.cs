using Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.UserUsecases
{
    public class DeleteUserUC
    {
        private readonly IServiceWrapper _services;
        public DeleteUserUC(IServiceWrapper services)
        {
            _services = services;
        }
        public async Task ExecuteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Invalid Id!");
                }
                var user = await _services.UserRepository.GetAsync(id);
                if (user == null)
                {
                    throw new ArgumentException($"User with Id {id} not found!");
                }
                await _services.UserRepository.LoadCollections(user, u => u.Tasks);
                // Tasks should be ToList() to not throw exception
                foreach (var task in user.Tasks.ToList())
                {
                    user.RemoveTaskEntity(task);
                    //user.Tasks.Remove(task);
                }

                user.IsActive = false;
                await _services.UserRepository.UpdateAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
