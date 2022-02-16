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

        public UnitOfWorkFake(TestData data)
        {
            _operations = new RepositoryFake<Operation>(data.Operations);
            _categories = new RepositoryFake<OperationCategory>(data.Categories);
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
