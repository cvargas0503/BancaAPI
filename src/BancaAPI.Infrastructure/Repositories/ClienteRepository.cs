// Infrastructure/Repositories/ClienteRepository.cs
using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Domain.Entities;
using BancaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BancaAPI.Infrastructure.Repositories
{
    public class ClienteRepository : IClientRepository
    {
        private readonly BancaDbContext _context;

        public ClienteRepository(BancaDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
