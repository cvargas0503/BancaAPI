using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancaAPI.Domain.Entities
{
    public class TipoTransaccion
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Descripcion { get; set; }

        // Ejemplos: "Depósito", "Retiro"
        public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
    }

}
