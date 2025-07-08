using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Domain.Entities;
using BancaAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly BancaDbContext _context;

        public TransaccionRepository(BancaDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaccion>> ObtenerPorCuentaAsync(Guid cuentaId)
        {
            return await _context.Transacciones
                .Include(t => t.TipoTransaccion)
                .Where(t => t.CuentaId == cuentaId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }
    }
}
