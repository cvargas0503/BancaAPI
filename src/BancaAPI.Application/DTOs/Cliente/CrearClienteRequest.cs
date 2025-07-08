using System.ComponentModel.DataAnnotations;

namespace BancaAPI.Application.DTOs.Cliente
{
    public class CrearClienteRequest
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public string Sexo { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Ingresos { get; set; }
    }
}
