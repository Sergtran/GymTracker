using GymTracker.Domain.Enums;

namespace GymTracker.Application.Dtos;

public record SessionExerciseDto(
	Guid Id, string Name, ExerciseType ExerciseType,
	Laterality Laterality, int DisplayOrder);