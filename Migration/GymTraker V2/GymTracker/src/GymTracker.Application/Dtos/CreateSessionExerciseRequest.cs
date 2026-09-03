using GymTracker.Domain.Enums;

namespace GymTracker.Application.Dtos;

public record CreateSessionExerciseRequest(
	string Name,
	ExerciseType ExerciseType,
	Laterality Laterality);