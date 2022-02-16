using Common.Models;
using DataAccess.Repositories;
using System;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HomeFinanceContext _context;
        private GenericRepository<Operation> _operations;
        private GenericRepository<OperationCategory> _categories;

        private bool _disposed = false;

        public IGenericRepository<Operation> Operations
        {
            get
            {
                _operations ??= new GenericRepository<Operation>(_context);
                return _operations;
            }
        }

        public IGenericRepository<OperationCategory> Categories
        {
            get
            {
                _categories ??= new GenericRepository<OperationCategory>(_context);
                return _categories;
            }
        }

        public UnitOfWork(HomeFinanceContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveAsync()
        {
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}