// Application/Validators/ConsultarSaldoRequestValidator.cs
using BancaAPI.Application.DTOs.Cuenta;
using FluentValidation;

namespace BancaAPI.Application.Validators
{
    public class ConsultarSaldoRequestValidator : AbstractValidator<ConsultarSaldoRequest>
    {
        public ConsultarSaldoRequestValidator()
        {
            RuleFor(x => x.NumeroCuenta)
                .NotEmpty().WithMessage("El número de cuenta es obligatorio.")
                .Length(10).WithMessage("El número de cuenta debe tener 10 caracteres.");
        }
    }
}
