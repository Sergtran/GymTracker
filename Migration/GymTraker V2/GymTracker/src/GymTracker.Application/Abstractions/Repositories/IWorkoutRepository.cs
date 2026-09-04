using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions.Repositories;

public interface IWorkoutRepository
{
	Task<Workout?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default);
	Task<(IReadOnlyList<Workout> Items, int TotalCount)> GetByUserPagedAsync(
		string userId, int page, int pageSize, CancellationToken ct = default);
	Task<decimal?> GetPreviousBestMetricAsync(
		string userId, string exerciseName, Laterality laterality,
		ExerciseType type, CancellationToken ct = default);
	Task<RoutineWorkoutSummary?> GetRoutineSummaryAsync(string userId, Guid routineId, CancellationToken ct = default);
	Task<IReadOnlyList<DateTime>> GetWorkoutDatesAsync(string userId, Guid routineId, CancellationToken ct = default);
	Task<IReadOnlyList<ExerciseFrequency>> GetExerciseFrequencyAsync(
		string userId, Guid routineId, int limit, CancellationToken ct = default);
	Task AddAsync(Workout workout, CancellationToken ct = default);
}
