using BancaAPI.Application.Interfaces.Common;
using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Application.Services;
using BancaAPI.Domain.Entities;
using Moq;
using FluentAssertions;
using BancaAPI.Application.Exceptions;

public class CuentaServiceTests
{
    private readonly Mock<ICuentaRepository> _cuentaRepoMock = new();
    private readonly Mock<ITransaccionRepository> _transaccionRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

    private readonly CuentaService _service;

    public CuentaServiceTests()
    {
        _service = new CuentaService(_cuentaRepoMock.Object, _transaccionRepoMock.Object, _unitOfWorkMock.Object);
    }

    [Fact(DisplayName = "API Permite crear cuenta bancaria")]
    public async Task CrearCuentaAsync_DeberiaRetornarCuentaDto()
    {
        var clienteId = Guid.NewGuid();
        _cuentaRepoMock.Setup(r => r.CrearAsync(It.IsAny<CuentaBancaria>()))
            .Returns(Task.CompletedTask);

        var resultado = await _service.CrearCuentaAsync(clienteId, 1000);

        resultado.Should().NotBeNull();
        resultado.Saldo.Should().Be(1000);
        resultado.NumeroCuenta.Should().NotBeNullOrWhiteSpace();
        _cuentaRepoMock.Verify(r => r.CrearAsync(It.IsAny<CuentaBancaria>()), Times.Once);
    }

    [Fact(DisplayName = "Al realizar un deposito se actualiza el saldo")]
    public async Task RealizarDepositoAsync_DeberiaActualizarSaldo()
    {
        var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), Saldo = 500 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);

        var monto = 200;

        await _service.RealizarDepositoAsync("1234567890", monto);

        cuenta.Saldo.Should().Be(700);
        _cuentaRepoMock.Verify(r => r.ActualizarAsync(It.IsAny<CuentaBancaria>()), Times.Once);
        _transaccionRepoMock.Verify(t => t.RegistrarAsync(It.IsAny<Transaccion>()), Times.Once);
    }

    [Fact(DisplayName = "Al realizar un retiro, se disminuye el saldo")]
    public async Task RealizarRetiroAsync_DeberiaReducirSaldo()
    {
        var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), Saldo = 1000 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);

        await _service.RealizarRetiroAsync("1234567890", 500);

        cuenta.Saldo.Should().Be(500);
        _cuentaRepoMock.Verify(r => r.ActualizarAsync(It.IsAny<CuentaBancaria>()), Times.Once);
        _transaccionRepoMock.Verify(t => t.RegistrarAsync(It.IsAny<Transaccion>()), Times.Once);
    }


    [Fact(DisplayName = "Al consultar el saldo disponible, retorna el saldo correcto")]
    public async Task ConsultarSaldoAsync_DeberiaRetornarSaldoActual()
    {
        var cuenta = new CuentaBancaria { Saldo = 1500 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("123")).ReturnsAsync(cuenta);

        var saldo = await _service.ConsultarSaldoAsync("123");

        saldo.Should().Be(1500);
    }

    [Fact(DisplayName = "Resumen muestra las transacciones correctamente")]
public async Task ObtenerResumenTransaccionesAsync_DeberiaRetornarTransaccionesPaginadas()
{
    var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), NumeroCuenta = "1234567890" };
    var transacciones = Enumerable.Range(1, 10).Select(i =>
        new Transaccion
        {
            Id = Guid.NewGuid(),
            CuentaId = cuenta.Id,
            Fecha = DateTime.UtcNow.AddDays(-i),
            Monto = 100 * i,
            TipoTransaccion = new TipoTransaccion { Descripcion = "Depósito" },
            SaldoDespues = 1000 + i * 100
        }).ToList();

    _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);
    _transaccionRepoMock.Setup(r => r.ObtenerPorCuentaAsync(cuenta.Id)).ReturnsAsync(transacciones);

    var result = await _service.ObtenerResumenTransaccionesAsync("1234567890", 1, 5);

    result.Should().NotBeNull();
    result.Items.Should().HaveCount(5);
    result.Total.Should().Be(10);
}

    [Fact(DisplayName = "Si no se cuenta con fondos suficientes para retirar, lanza excepción")]
    public async Task RealizarRetiroAsync_FondosInsuficientes_DeberiaLanzarExcepcion()
    {
        var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), Saldo = 100 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);

        Func<Task> accion = async () => await _service.RealizarRetiroAsync("1234567890", 200);

        await accion.Should().ThrowAsync<InsufficientFundsException>();
        _cuentaRepoMock.Verify(r => r.ActualizarAsync(It.IsAny<CuentaBancaria>()), Times.Never);
        _transaccionRepoMock.Verify(t => t.RegistrarAsync(It.IsAny<Transaccion>()), Times.Never);
    }
    [Theory(DisplayName = "Al ingresar un monto inválido no permite hacer el deposito")]
    [InlineData(0)]
    [InlineData(-50)]
    public async Task RealizarDepositoAsync_MontoInvalido_DeberiaLanzarExcepcion(decimal monto)
    {
        var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), Saldo = 100 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);

        Func<Task> accion = async () => await _service.RealizarDepositoAsync("1234567890", monto);

        await accion.Should().ThrowAsync<InvalidTransactionAmountException>();
        _cuentaRepoMock.Verify(r => r.ActualizarAsync(It.IsAny<CuentaBancaria>()), Times.Never);
        _transaccionRepoMock.Verify(t => t.RegistrarAsync(It.IsAny<Transaccion>()), Times.Never);
    }

    [Fact(DisplayName = "Si no existe una cuenta, lanza error")]
    public async Task ConsultarSaldoAsync_CuentaInexistente_DeberiaLanzarExcepcion()
    {
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("0000")).ReturnsAsync((CuentaBancaria?)null);

        Func<Task> accion = async () => await _service.ConsultarSaldoAsync("0000");

        await accion.Should().ThrowAsync<NotFoundException>();
    }

    [Fact(DisplayName = "Al realizar un deposito, la transacción se registra correctamente")]
    public async Task RealizarDepositoAsync_DeberiaRegistrarTransaccionCorrectamente()
    {
        var cuenta = new CuentaBancaria { Id = Guid.NewGuid(), Saldo = 500 };
        _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("1234567890")).ReturnsAsync(cuenta);

        Transaccion? transaccionRegistrada = null;
        _transaccionRepoMock
            .Setup(t => t.RegistrarAsync(It.IsAny<Transaccion>()))
            .Callback<Transaccion>(t => transaccionRegistrada = t)
            .Returns(Task.CompletedTask);

        await _service.RealizarDepositoAsync("1234567890", 300);

        transaccionRegistrada.Should().NotBeNull();
        transaccionRegistrada!.CuentaId.Should().Be(cuenta.Id);
        transaccionRegistrada.Monto.Should().Be(300);
        transaccionRegistrada.SaldoDespues.Should().Be(800);
    }
}
