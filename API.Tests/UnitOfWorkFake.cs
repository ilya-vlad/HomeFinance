using Common.Models;
using DataAccess;
using DataAccess.Repositories;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Tests
{
    public class UnitOfWorkFake : IUnitOfWork
    {
        private IGenericRepository<Operation> _operations;
        private IGenericRepository<OperationCategory> _categories;

        public IGenericRepository<Operation> Operations => _operations;

        public IGenericRepository<OperationCategory> Categories => _categories;

        public UnitOfWorkFake(List<Operation> operations, List<OperationCategory> categories)
        {
            _operations = new RepositoryFake<Operation>(operations);
            _categories = new RepositoryFake<OperationCategory>(categories);
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            return true;
        }
    }
}
