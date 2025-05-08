

using System.Linq.Expressions;

namespace BL.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<T>> Filter(Expression<Func<T,bool>> filter);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        void Delete(T entity);
    }
   
}
