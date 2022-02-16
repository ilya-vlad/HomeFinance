using Common.Models;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    public class RepositoryFake<T> : IGenericRepository<T> where T : BaseEntity
    {
        private List<T> _collection { get; set; }

        public RepositoryFake(List<T> collection)
        {
            _collection = new(collection);
        }

        public IQueryable<T> GetAll()
        {
            return _collection.AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return _collection.FirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> AddAsync(T entity)
        {
            _collection.Add(entity);
            return true;
        }

        public void Update(T entity)
        {
            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = _collection.FirstOrDefault(x => x.Id == id);
            _collection.Remove(item);
            return true;
        }
    }
}
