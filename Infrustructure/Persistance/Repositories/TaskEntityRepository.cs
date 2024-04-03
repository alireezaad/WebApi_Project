using Domain.Entities;
using Domain.IRepositories;
using Domain.Repositories;
using Infrustructure.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class TaskEntityRepository : BaseRepository<TaskEntity>, ITaskEntityRepository
    {
        public TaskEntityRepository(ApiDbContext dbCOntext) : base(dbCOntext)
        {

        }
        //private readonly ApiDbContext _dbContext;
        //public TaskEntityRepository(ApiDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //public async Task<TaskEntity> AddAsync(TaskEntity taskEntity)
        //{
        //    _dbContext.TaskEntities.Add(taskEntity);
        //    await _dbContext.SaveChangesAsync();
        //    return taskEntity;
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    var taskEntity = await GetAsync(id);
        //    _dbContext.Remove(taskEntity);
        //    await _dbContext.SaveChangesAsync();
        //}

        //public async Task<TaskEntity> GetAsync(int id)
        //{
        //    var taskEntity = await _dbContext.TaskEntities.FindAsync(id);
        //    if (taskEntity == null)
        //    {
        //        throw new InvalidOperationException($"Task with ID {id} not found.");
        //    }
        //    return taskEntity;
        //}

        //public async Task<List<TaskEntity>> GetAllAsync()
        //{
        //    return await _dbContext.TaskEntities.Include(t => t.Users).ToListAsync();
        //}

        //public async Task<TaskEntity> UpdateAsync(TaskEntity taskEntity)
        //{
        //    _dbContext.Entry(taskEntity).State = EntityState.Modified;
        //    await _dbContext.SaveChangesAsync();
        //    return taskEntity;
        //}

        //public async Task LoadUsersAsync(TaskEntity taskEntity)
        //{
        //    await _dbContext.Entry(taskEntity).Collection(t => t.Users).LoadAsync();
        //}

    }
}
