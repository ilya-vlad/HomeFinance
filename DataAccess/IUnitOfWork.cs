using Common.Models;
using DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Operation> Operations { get; }
        public IGenericRepository<OperationCategory> Categories { get; }

        public Task<bool> SaveAsync();
    }
}