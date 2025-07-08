// API/Controllers/ClienteController.cs
using BancaAPI.Application.DTOs.Cliente;
using BancaAPI.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BancaAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(IClienteService clienteService, ILogger<ClienteController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Crea un nuevo perfil de cliente
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CrearCliente([FromBody] CrearClienteRequest request)
        {
            try
            {
                var cliente = await _clienteService.CrearClienteAsync(request);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el cliente");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
    }
}
