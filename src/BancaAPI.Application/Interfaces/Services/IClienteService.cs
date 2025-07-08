// Application/Interfaces/IClienteService.cs
using BancaAPI.Application.DTOs.Cliente;
using System.Threading.Tasks;

namespace BancaAPI.Application.Interfaces.Services
{
    public interface IClienteService
    {
        Task<ClienteDto> CrearClienteAsync(CrearClienteRequest request);
    }
}
