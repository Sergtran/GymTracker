using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Domain.Entities;
using GymTracker.Domain.ValueObjects;
using GymTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence.Repositories;

public sealed class EfRoutineRepository : IRoutineRepository
{
	private readonly GymTrackerDbContext _db;

	public EfRoutineRepository(GymTrackerDbContext db)
		=> _db = db;

	public async Task<Routine?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default)
		=> await _db.Routines
			.AsNoTracking()
			.FirstOrDefaultAsync(r => r.UserId == userId && r.Id == id, ct);

	public async Task<IReadOnlyList<Routine>> GetByUserAsync(string userId, CancellationToken ct = default)
		=> await _db.Routines
			.AsNoTracking()
			.Where(r => r.UserId == userId)
			.OrderBy(r => r.CreatedAt)
			.ToListAsync(ct);

	public async Task<bool> ExistsByNameAsync(string userId, string name, CancellationToken ct = default)
		=> await _db.Routines
			.AnyAsync(r => r.UserId == userId && r.Name == new Name(name), ct);

	public async Task AddAsync(Routine routine, CancellationToken ct = default)
	{
		_db.Routines.Add(routine);
		await _db.SaveChangesAsync(ct);
	}
}