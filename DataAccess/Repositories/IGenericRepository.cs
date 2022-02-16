using Common.Models;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(int id);

        Task<bool> AddAsync(T entity);

        void Update(T entity);

        Task<bool> DeleteAsync(int id);
    }
}