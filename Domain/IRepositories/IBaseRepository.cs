using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.IRepositories
{
    public interface IBaseRepository<T>
    {
        Task<T> GetAsync(int id);
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[]? includes);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        //Task DeleteAsync(User user);
        Task LoadCollections(T entity, params Expression<Func<T, IEnumerable<object>>>[]? collectionIncludes);
    }
}
