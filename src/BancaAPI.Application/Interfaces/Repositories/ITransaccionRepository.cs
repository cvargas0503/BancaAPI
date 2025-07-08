using BancaAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Interfaces.Repositories
{
    public interface ITransaccionRepository
    {
        Task RegistrarAsync(Transaccion transaccion);
        Task<List<Transaccion>> ObtenerPorCuentaAsync(Guid cuentaId);
    }
}
