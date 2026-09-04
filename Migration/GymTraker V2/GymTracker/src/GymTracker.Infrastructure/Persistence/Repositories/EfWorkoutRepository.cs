using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;
using GymTracker.Infrastructure.Data;
using GymTracker.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence.Repositories;

public sealed class EfWorkoutRepository : IWorkoutRepository
{
	private readonly GymTrackerDbContext _db;

	public EfWorkoutRepository(GymTrackerDbContext db)
		=> _db = db;

	public async Task<Workout?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default)
		=> await _db.Workouts
			.AsNoTracking()
			.Include(w => w.Exercises)
				.ThenInclude(e => e.Sets)
			.FirstOrDefaultAsync(w => w.UserId == userId && w.Id == id, ct);

	public async Task<(IReadOnlyList<Workout> Items, int TotalCount)> GetByUserPagedAsync(
		string userId, int page, int pageSize, CancellationToken ct = default)
	{
		var query = _db.Workouts
			.Where(w => w.UserId == userId)
			.OrderByDescending(w => w.WorkoutDate)
			.ThenByDescending(w => w.CreatedAt);

		var totalCount = await query.CountAsync(ct);

		var items = await query
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Include(w => w.Exercises)
				.ThenInclude(e => e.Sets)
			.ToListAsync(ct);

		return (items, totalCount);
	}

    public async Task<decimal?> GetPreviousBestMetricAsync(
        string userId, string exerciseName, Laterality laterality,
        ExerciseType type, CancellationToken ct = default)
    {
        var sets = await _db.Workouts
            .Where(w => w.UserId == userId)
            .SelectMany(w => w.Exercises)
            .Where(e => e.Name == new Name(exerciseName) && e.Laterality == laterality)
            .SelectMany(e => e.Sets)
            .Select(s => new { Reps = s.Reps, Weight = s.Weight })
            .ToListAsync(ct);

        if (sets.Count == 0)
            return null;

        return type == ExerciseType.Bodyweight
            ? sets.Max(s => (decimal)s.Reps.Value)
            : sets.Max(s => s.Weight.Value);
    }

	public async Task AddAsync(Workout workout, CancellationToken ct = default)
	{
		_db.Workouts.Add(workout);
		await _db.SaveChangesAsync(ct);
	}

	public async Task<RoutineWorkoutSummary?> GetRoutineSummaryAsync(
		string userId, Guid routineId, CancellationToken ct = default)
	{
		var query = _db.Workouts.Where(w => w.UserId == userId && w.RoutineId == routineId);

		var count = await query.CountAsync(ct);
		if (count == 0)
			return null;

		var first = await query.MinAsync(w => (DateTime?)w.WorkoutDate, ct);
		var last = await query.MaxAsync(w => (DateTime?)w.WorkoutDate, ct);
		var prCount = await query
			.SelectMany(w => w.Exercises)
			.CountAsync(e => e.PrStatus == PrStatus.New, ct);

		return new RoutineWorkoutSummary(count, first, last, prCount);
	}

	public async Task<IReadOnlyList<DateTime>> GetWorkoutDatesAsync(
		string userId, Guid routineId, CancellationToken ct = default)
		=> await _db.Workouts
			.Where(w => w.UserId == userId && w.RoutineId == routineId)
			.Select(w => w.WorkoutDate)
			.ToListAsync(ct);

	public async Task<IReadOnlyList<ExerciseFrequency>> GetExerciseFrequencyAsync(
		string userId, Guid routineId, int limit, CancellationToken ct = default)
	{
		var rows = await _db.Workouts
			.Where(w => w.UserId == userId && w.RoutineId == routineId)
			.SelectMany(w => w.Exercises)
			.GroupBy(e => e.Name)
			.OrderByDescending(g => g.Count())
			.Take(limit)
			.Select(g => new { Name = g.Key, Count = g.Count() })
			.ToListAsync(ct);

		return rows.Select(r => new ExerciseFrequency(r.Name.Value, r.Count)).ToList();
	}
}
