// Application/Validators/CrearClienteRequestValidator.cs
using BancaAPI.Application.DTOs.Cliente;
using FluentValidation;
using System;

namespace BancaAPI.Application.Validators
{
    public class CrearClienteRequestValidator : AbstractValidator<CrearClienteRequest>
    {
        public CrearClienteRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(x => x.FechaNacimiento)
                .LessThan(DateTime.Today).WithMessage("La fecha de nacimiento debe ser anterior a hoy.");

            RuleFor(x => x.Sexo)
                .NotEmpty().WithMessage("El sexo es obligatorio.")
                .Must(s => s == "Masculino" || s == "Femenino" || s == "Otro")
                .WithMessage("El sexo debe ser 'Masculino', 'Femenino' o 'Otro'.");

            RuleFor(x => x.Ingresos)
                .GreaterThanOrEqualTo(0).WithMessage("Los ingresos no pueden ser negativos.");
        }
    }
}
