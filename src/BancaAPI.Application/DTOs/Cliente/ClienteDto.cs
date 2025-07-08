// Application/DTOs/ClienteDto.cs
using System;

namespace BancaAPI.Application.DTOs.Cliente
{
    public class ClienteDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public decimal Ingresos { get; set; }
    }
}
