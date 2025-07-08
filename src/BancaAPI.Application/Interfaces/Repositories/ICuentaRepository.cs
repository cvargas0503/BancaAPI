using BancaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Interfaces.Repositories
{
    public interface ICuentaRepository
    {
        Task<CuentaBancaria> ObtenerPorNumeroAsync(string numeroCuenta);
        Task CrearAsync(CuentaBancaria cuenta);
        Task ActualizarAsync(CuentaBancaria cuenta);
        Task<List<CuentaBancaria>> ListarPaginadoAsync(int page, int pageSize);
        Task<int> ContarTotalAsync();

    }
}
