using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Domain.Entities;
using GymTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymTracker.Infrastructure.Persistence.Repositories;

public sealed class EfUserSettingsRepository : IUserSettingsRepository
{
	private readonly GymTrackerDbContext _db;

	public EfUserSettingsRepository(GymTrackerDbContext db)
		=> _db = db;

	public async Task<UserSettings?> GetByUserIdAsync(string userId, CancellationToken ct = default)
		=> await _db.UserSettings
			.AsTracking()
			.FirstOrDefaultAsync(s => s.UserId == userId, ct);

	public async Task AddAsync(UserSettings settings, CancellationToken ct = default)
	{
		_db.UserSettings.Add(settings);
		await _db.SaveChangesAsync(ct);
	}

	public async Task UpdateAsync(UserSettings settings, CancellationToken ct = default)
		=> await _db.SaveChangesAsync(ct);
}
