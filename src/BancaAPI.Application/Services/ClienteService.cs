using BancaAPI.Application.DTOs.Cliente;
using BancaAPI.Application.Interfaces.Repositories;
using BancaAPI.Application.Interfaces.Services;
using BancaAPI.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace BancaAPI.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClientRepository _clienteRepository;

        public ClienteService(IClientRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ClienteDto> CrearClienteAsync(CrearClienteRequest request)
        {
            var cliente = new Cliente
            {
                Id = Guid.NewGuid(),
                Nombre = request.Nombre,
                FechaNacimiento = request.FechaNacimiento,
                Sexo = request.Sexo,
                Ingresos = request.Ingresos
            };

            await _clienteRepository.CrearAsync(cliente);

            return new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                FechaNacimiento = cliente.FechaNacimiento,
                Sexo = cliente.Sexo,
                Ingresos = cliente.Ingresos
            };
        }
    }
}
