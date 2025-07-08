using System.ComponentModel.DataAnnotations;

public class RetiroRequest
{
    [Required]
    [StringLength(20, MinimumLength = 6)]
    public string NumeroCuenta { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
    public decimal Monto { get; set; }
}
