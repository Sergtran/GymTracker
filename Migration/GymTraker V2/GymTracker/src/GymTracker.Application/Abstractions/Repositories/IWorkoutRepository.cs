using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;

namespace GymTracker.Application.Abstractions.Repositories;

public interface IWorkoutRepository
{
	Task<Workout?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default);
	Task<(IReadOnlyList<Workout> Items, int TotalCount)> GetByUserPagedAsync(
		string userId, int page, int pageSize, CancellationToken ct = default);
	Task<decimal?> GetPreviousBestMetricAsync(
		string userId, string exerciseName, Laterality laterality,
		ExerciseType type, CancellationToken ct = default);
	Task AddAsync(Workout workout, CancellationToken ct = default);
}