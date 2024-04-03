using Domain.Entities;
using Domain.IRepositories;
using Infrustructure.Persistance.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrustructure.Persistance.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApiDbContext _dbCOntext;
        public BaseRepository(ApiDbContext dbCOntext)
        {
            _dbCOntext = dbCOntext;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbCOntext.Set<T>().Add(entity);
            await _dbCOntext.SaveChangesAsync();
            return entity;
        }

        public async Task<List<T>> GetAllAsync(params Expression<Func<T,object>>[]? includes)
        {
            var entityList = _dbCOntext.Set<T>().AsQueryable();
            if (includes?.Length >= 1)
            {
                foreach (var include in includes)
                {
                    entityList = entityList.Include(include);
                }
            }
            return await entityList.AsNoTracking().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _dbCOntext.Set<T>().FindAsync(id);
        }

        public async Task LoadCollections(T entity, params Expression<Func<T, IEnumerable<object>>>[]? collectionIncludes)
        {
            foreach (var include in collectionIncludes)
            {
                var propertyName = ((MemberExpression)include.Body).Member.Name;
                var propertyInfo = typeof(T).GetProperty(propertyName);

                if (propertyInfo != null)
                {
                    var collection = propertyInfo.GetValue(entity) as IEnumerable<object>;
                    if (collection != null)
                    {
                        await _dbCOntext.Entry(entity).Collection(propertyName).LoadAsync();
                    }
                }
            }
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbCOntext.Set<T>().Entry(entity).State = EntityState.Modified;
            await _dbCOntext.SaveChangesAsync();
            return entity;
        }
    }
}
