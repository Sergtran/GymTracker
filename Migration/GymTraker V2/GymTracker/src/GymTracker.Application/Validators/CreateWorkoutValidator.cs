using FluentValidation;
using GymTracker.Application.Dtos;
using GymTracker.Domain.Entities;

namespace GymTracker.Application.Validators;

public sealed class CreateWorkoutValidator : AbstractValidator<CreateWorkoutRequest>
{
	public CreateWorkoutValidator()
	{
		RuleFor(x => x.RoutineName).NotEmpty().MaximumLength(Routine.MaxNameLength);
		RuleFor(x => x.SessionName).NotEmpty().MaximumLength(WorkoutSession.MaxNameLength);
		RuleFor(x => x.WorkoutDate).NotEmpty();

		RuleFor(x => x.Exercises)
			.NotEmpty().WithMessage("El entrenamiento debe tener al menos un ejercicio.");

		RuleForEach(x => x.Exercises).ChildRules(exercise =>
		{
			exercise.RuleFor(e => e.Name).NotEmpty().MaximumLength(WorkoutExercise.MaxNameLength);
			exercise.RuleFor(e => e.ExerciseType).IsInEnum();
			exercise.RuleFor(e => e.Laterality).IsInEnum();
			exercise.RuleFor(e => e.Sets)
				.NotEmpty().WithMessage("Cada ejercicio debe tener al menos una serie.");
		});

		// Validación anidada de series con RuleForEach + SetValidator del tipo de serie
		RuleForEach(x => x.Exercises.SelectMany(e => e.Sets))
			.SetValidator(new WorkoutSetValidator());
	}
}