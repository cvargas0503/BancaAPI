using BancaAPI.Application.DTOs.Common;
using BancaAPI.Application.DTOs.Cuenta;
using BancaAPI.Application.Exceptions;
using BancaAPI.Application.Interfaces.Common;
using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Application.Interfaces.Services;
using BancaAPI.Application.Mappers;
using BancaAPI.Domain.Entities;
using BancaAPI.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Application.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly ITransaccionRepository _transaccionRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CuentaService(ICuentaRepository cuentaRepo, ITransaccionRepository transaccionRepo, IUnitOfWork unitOfWork)
        {
            _cuentaRepo = cuentaRepo;
            _transaccionRepo = transaccionRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<CuentaDto> CrearCuentaAsync(Guid clienteId, decimal saldoInicial)
        {
            if (clienteId == Guid.Empty)
                throw new BusinessRuleException("El identificador del cliente es obligatorio.");

            if (saldoInicial < 0)
                throw new BusinessRuleException("El saldo inicial no puede ser negativo.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cuenta = new CuentaBancaria
                {
                    Id = Guid.NewGuid(),
                    ClienteId = clienteId,
                    NumeroCuenta = Guid.NewGuid().ToString("N")[..10], // Substring más limpio
                    Saldo = saldoInicial
                };

                await _cuentaRepo.CrearAsync(cuenta);

                await _unitOfWork.CommitTransactionAsync();

                return new CuentaDto
                {
                    NumeroCuenta = cuenta.NumeroCuenta,
                    Saldo = cuenta.Saldo
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<decimal> ConsultarSaldoAsync(string numeroCuenta)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                throw new BusinessRuleException("Debe proporcionar un número de cuenta válido.");

            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);

            if (cuenta == null)
                throw new NotFoundException("Account", numeroCuenta);

            return cuenta.Saldo;
        }


        public async Task RealizarDepositoAsync(string numeroCuenta, decimal monto)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                throw new BusinessRuleException("Número de cuenta es requerido.");

            if (monto <= 0)
                throw new InvalidTransactionAmountException(monto);

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
                if (cuenta == null)
                    throw new NotFoundException("Account", numeroCuenta);

                cuenta.Saldo += monto;
                await _cuentaRepo.ActualizarAsync(cuenta);

                var transaccion = new Transaccion
                {
                    Id = Guid.NewGuid(),
                    CuentaId = cuenta.Id,
                    TipoTransaccionId = TipoTransaccionMapper.ObtenerId(TipoTransaccionEnum.Deposito),
                    Monto = monto,
                    SaldoDespues = cuenta.Saldo,
                    Fecha = DateTime.UtcNow
                };

                await _transaccionRepo.RegistrarAsync(transaccion);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task RealizarRetiroAsync(string numeroCuenta, decimal monto)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                throw new BusinessRuleException("El número de cuenta es obligatorio.");

            if (monto <= 0)
                throw new BusinessRuleException("El monto del retiro debe ser mayor a cero.");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta);
                if (cuenta == null)
                    throw new NotFoundException("Account", numeroCuenta);

                if (cuenta.Saldo < monto)
                    throw new InsufficientFundsException(numeroCuenta, cuenta.Saldo, monto);

                cuenta.Saldo -= monto;
                await _cuentaRepo.ActualizarAsync(cuenta);

                var transaccion = new Transaccion
                {
                    Id = Guid.NewGuid(),
                    CuentaId = cuenta.Id,
                    TipoTransaccionId = TipoTransaccionMapper.ObtenerId(TipoTransaccionEnum.Retiro),
                    Monto = monto,
                    SaldoDespues = cuenta.Saldo,
                    Fecha = DateTime.UtcNow
                };

                await _transaccionRepo.RegistrarAsync(transaccion);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<PaginatedResult<TransaccionListaDto>> ObtenerResumenTransaccionesAsync(string numeroCuenta, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                throw new BusinessRuleException("Debe proporcionar un número de cuenta válido.");

            if (page <= 0 || pageSize <= 0)
                throw new BusinessRuleException("Los parámetros de paginación deben ser mayores a cero.");

            var cuenta = await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta)
                         ?? throw new NotFoundException("Cuenta bancaria", numeroCuenta);

            var transacciones = await _transaccionRepo.ObtenerPorCuentaAsync(cuenta.Id);

            var total = transacciones.Count;
            var paginadas = transacciones
                .OrderByDescending(t => t.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransaccionListaDto
                {
                    Id = t.Id,
                    Tipo = t.TipoTransaccion.Descripcion,
                    Monto = t.Monto,
                    SaldoDespues = t.SaldoDespues,
                    Fecha = t.Fecha
                }).ToList();

            return new PaginatedResult<TransaccionListaDto>
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                Items = paginadas
            };
        }

    }

}
