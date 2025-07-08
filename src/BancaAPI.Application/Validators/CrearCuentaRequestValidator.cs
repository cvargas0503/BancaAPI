using BancaAPI.Application.DTOs.Cuenta;
using FluentValidation;

namespace BancaAPI.Application.Validators
{
    public class CrearCuentaRequestValidator : AbstractValidator<CrearCuentaRequest>
    {
        public CrearCuentaRequestValidator()
        {
            RuleFor(x => x.ClienteId).NotEmpty().WithMessage("El cliente es obligatorio.");
            RuleFor(x => x.SaldoInicial).GreaterThanOrEqualTo(0).WithMessage("El saldo inicial no puede ser negativo.");
        }
    }
}
