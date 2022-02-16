using Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly HomeFinanceContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(HomeFinanceContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<bool> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _context.Remove(entity);
            return true;
        }

        public void Update(T entity)
        {            
            _context.Entry(entity).State = EntityState.Modified;            
        }
    }
}