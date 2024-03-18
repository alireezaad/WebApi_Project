using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IRepositories;

namespace Domain.Repositories
{
    public interface ITaskEntityRepository : IBaseRepository<TaskEntity>
    {
        //Task<TaskEntity> GetAsync(int id);
        //Task<List<TaskEntity>> GetAllAsync();
        //Task<TaskEntity> AddAsync(TaskEntity taskEntity);
        //Task<TaskEntity> UpdateAsync(TaskEntity taskEntity);
        //Task DeleteAsync(int id);
        //Task LoadUsersAsync(TaskEntity taskEntity);
    }
}
