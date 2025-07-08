using BancaAPI.API.Filters;
using BancaAPI.Application.DTOs.Common;
using BancaAPI.Application.DTOs.Cuenta;
using BancaAPI.Application.Exceptions;
using BancaAPI.Application.Helpers;
using BancaAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BancaAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;
        private readonly ILogger<CuentaController> _logger;

        public CuentaController(ICuentaService cuentaService, ILogger<CuentaController> logger)
        {
            _cuentaService = cuentaService;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nueva cuenta bancaria
        /// </summary>
        /// <param name="request">Datos de la cuenta a crear</param>
        /// <returns>Cuenta creada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(CuentaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ServiceFilter(typeof(AntiSpamFilter))]
        public async Task<IActionResult> CrearCuenta([FromBody] CrearCuentaRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Solicitud inválida al crear cuenta: {@ModelState}", ModelState);
                return ValidationProblem(ModelState);
            }

            try
            {
                var cuenta = await _cuentaService.CrearCuentaAsync(request.ClienteId, request.SaldoInicial);
                return CreatedAtAction(nameof(ConsultarSaldo), new { numeroCuenta = cuenta.NumeroCuenta }, cuenta);
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al crear cuenta");
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Regla de negocio violada",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (DuplicateAccountException ex)
            {
                _logger.LogWarning(ex, "Intento de crear cuenta duplicada");

                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Cuenta duplicada",
                    detail: ex.Message,
                    status: StatusCodes.Status409Conflict,
                    instance: HttpContext.Request.Path
                );

                return Conflict(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cuenta");

                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Error interno del servidor",
                    detail: "Ocurrió un error inesperado al crear la cuenta.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: HttpContext.Request.Path
                );

                return StatusCode(500, problem);
            }
        }

        /// <summary>
        /// Consulta el saldo actual de una cuenta bancaria
        /// </summary>
        /// <returns>Saldo actual</returns>
        [HttpGet("[Controller]/{numeroCuenta}/saldo")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConsultarSaldo([FromRoute] string numeroCuenta)
        {
            if (string.IsNullOrWhiteSpace(numeroCuenta))
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Número de cuenta inválido",
                    detail: "Debe proporcionar un número de cuenta válido.",
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }

            try
            {
                var saldo = await _cuentaService.ConsultarSaldoAsync(numeroCuenta);
                return Ok(new { numeroCuenta, saldo });
            }
            catch (BusinessRuleException ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada al consultar saldo");
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Regla de negocio violada",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Cuenta no encontrada: {NumeroCuenta}", numeroCuenta);
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Cuenta no encontrada",
                    detail: ex.Message,
                    status: StatusCodes.Status404NotFound,
                    instance: HttpContext.Request.Path
                );
                return NotFound(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar saldo");

                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Error interno del servidor",
                    detail: "Se produjo un error inesperado al consultar el saldo de la cuenta.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: HttpContext.Request.Path
                );

                return StatusCode(500, problem);
            }
        }

        /// <summary>
        /// Realiza un depósito en la cuenta bancaria
        /// </summary>
        [HttpPost("deposito")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RealizarDeposito([FromBody] DepositoRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                await _cuentaService.RealizarDepositoAsync(request.NumeroCuenta, request.Monto);
                return Ok(new { mensaje = "Depósito realizado con éxito." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(ErrorResponseFactory.CreateProblemDetails(
                    title: "Cuenta no encontrada",
                    detail: ex.Message,
                    status: StatusCodes.Status404NotFound,
                    instance: HttpContext.Request.Path
                ));
            }
            catch (InvalidTransactionAmountException ex)
            {
                return BadRequest(ErrorResponseFactory.CreateProblemDetails(
                    title: "Monto inválido",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                ));
            }
            catch (BusinessRuleException ex)
            {
                return UnprocessableEntity(ErrorResponseFactory.CreateProblemDetails(
                    title: "Regla de negocio violada",
                    detail: ex.Message,
                    status: StatusCodes.Status422UnprocessableEntity,
                    instance: HttpContext.Request.Path
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al realizar depósito");
                return StatusCode(500, ErrorResponseFactory.CreateProblemDetails(
                    title: "Error interno del servidor",
                    detail: "Ocurrió un error inesperado al procesar la solicitud.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: HttpContext.Request.Path
                ));
            }
        }


        /// <summary>
        /// Realiza un retiro de la cuenta bancaria
        /// </summary>
        [HttpPost("retiro")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RealizarRetiro([FromBody] RetiroRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                await _cuentaService.RealizarRetiroAsync(request.NumeroCuenta, request.Monto);
                return Ok(new { mensaje = "Retiro realizado con éxito." });
            }
            catch (BusinessRuleException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Regla de negocio violada",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (InvalidTransactionAmountException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Monto de retiro inválido",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (InsufficientFundsException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Fondos insuficientes",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (NotFoundException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Cuenta no encontrada",
                    detail: ex.Message,
                    status: StatusCodes.Status404NotFound,
                    instance: HttpContext.Request.Path
                );
                return NotFound(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al realizar el retiro");

                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Error interno del servidor",
                    detail: "Ocurrió un error inesperado al procesar la solicitud de retiro.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: HttpContext.Request.Path
                );

                return StatusCode(500, problem);
            }
        }

        [HttpGet("{numeroCuenta}/transacciones")]
        [ProducesResponseType(typeof(PaginatedResult<TransaccionListaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObtenerResumenTransacciones(
    [FromRoute] string numeroCuenta,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
        {
            try
            {
                var resumen = await _cuentaService.ObtenerResumenTransaccionesAsync(numeroCuenta, page, pageSize);
                return Ok(resumen);
            }
            catch (NotFoundException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Cuenta no encontrada",
                    detail: ex.Message,
                    status: StatusCodes.Status404NotFound,
                    instance: HttpContext.Request.Path
                );
                return NotFound(problem);
            }
            catch (BusinessRuleException ex)
            {
                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Regla de negocio violada",
                    detail: ex.Message,
                    status: StatusCodes.Status400BadRequest,
                    instance: HttpContext.Request.Path
                );
                return BadRequest(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener el resumen de transacciones");

                var problem = ErrorResponseFactory.CreateProblemDetails(
                    title: "Error interno del servidor",
                    detail: "Ocurrió un error inesperado al consultar el historial de transacciones.",
                    status: StatusCodes.Status500InternalServerError,
                    instance: HttpContext.Request.Path
                );

                return StatusCode(500, problem);
            }
        }

    }
}
