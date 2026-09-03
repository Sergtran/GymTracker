using FluentValidation;
using GymTracker.Application.Dtos;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Validators;

public sealed class CreateSessionExerciseRequestValidator : AbstractValidator<CreateSessionExerciseRequest>
{
	public CreateSessionExerciseRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("El nombre del ejercicio es obligatorio.")
			.MaximumLength(SessionExercise.MaxNameLength)
			.WithMessage($"El nombre no puede superar {SessionExercise.MaxNameLength} caracteres.");

		RuleFor(x => x.ExerciseType).IsInEnum();
		RuleFor(x => x.Laterality).IsInEnum();
	}
}