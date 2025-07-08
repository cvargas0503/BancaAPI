using BancaAPI.Application.Interfaces.Common;
using BancaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancaAPI.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BancaDbContext _context;

        public UnitOfWork(BancaDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if (_context.Database.CurrentTransaction == null)
                await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
