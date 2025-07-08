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
    public class CuentaRepository : ICuentaRepository
    {
        private readonly BancaDbContext _context;

        public CuentaRepository(BancaDbContext context) => _context = context;

        public async Task CrearAsync(CuentaBancaria cuenta)
        {
            _context.Cuentas.Add(cuenta);
            await _context.SaveChangesAsync();
        }

        public async Task<CuentaBancaria> ObtenerPorNumeroAsync(string numeroCuenta)
        {
            return await _context.Cuentas
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        public async Task ActualizarAsync(CuentaBancaria cuenta)
        {
            _context.Cuentas.Update(cuenta);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CuentaBancaria>> ListarPaginadoAsync(int page, int pageSize)
        {
            return await _context.Cuentas
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> ContarTotalAsync()
        {
            return await _context.Cuentas.CountAsync();
        }

    }

}
