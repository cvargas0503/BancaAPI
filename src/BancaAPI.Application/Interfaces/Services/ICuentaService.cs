using BancaAPI.Application.DTOs.Common;
using BancaAPI.Application.DTOs.Cuenta;

namespace BancaAPI.Application.Interfaces.Services
{
    public interface ICuentaService
    {
        Task<CuentaDto> CrearCuentaAsync(Guid clienteId, decimal saldoInicial);
        Task<decimal> ConsultarSaldoAsync(string numeroCuenta);
        Task RealizarDepositoAsync(string numeroCuenta, decimal monto);
        Task RealizarRetiroAsync(string numeroCuenta, decimal monto);
        Task<PaginatedResult<TransaccionListaDto>> ObtenerResumenTransaccionesAsync(string numeroCuenta, int page, int pageSize);
    }
}
