

using System.Linq.Expressions;
using BL.IRepositories;
using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class GenericRepository<T> : BL.IRepositories.IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> _dbset;
        public GenericRepository(ApplicationDbContext dbContext) { 
        
            _dbContext = dbContext;
            this._dbset= dbContext.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task<IEnumerable<T>> Filter(Expression<Func<T, bool>> filter)
        {
            return await _dbset.Where(filter).ToListAsync();
        }
        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            this._dbset.Attach(entity);
            this._dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
