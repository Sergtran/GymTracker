using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Domain.Entities;
using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;
using GymTracker.Infrastructure.Data;
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
		var sets = _db.Workouts
			.Where(w => w.UserId == userId)
			.SelectMany(w => w.Exercises)
			.Where(e => e.Name == new Name(exerciseName) && e.Laterality == laterality)
			.SelectMany(e => e.Sets);

		return type == ExerciseType.Bodyweight
			? await sets.MaxAsync(s => (decimal?)s.Reps.Value, ct)
			: await sets.MaxAsync(s => (decimal?)s.Weight.Value, ct);
	}

	public async Task AddAsync(Workout workout, CancellationToken ct = default)
	{
		_db.Workouts.Add(workout);
		await _db.SaveChangesAsync(ct);
	}
}