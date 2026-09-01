using FluentValidation;
using GymTracker.Application.Dtos;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Validators;

public sealed class CreateRoutineValidator : AbstractValidator<CreateRoutineRequest>
{
	public CreateRoutineValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("El nombre de la rutina es obligatorio.")
			.MaximumLength(Routine.MaxNameLength)
			.WithMessage($"El nombre no puede superar {Routine.MaxNameLength} caracteres.");
	}
}