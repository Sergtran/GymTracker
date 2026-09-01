using GymTracker.Domain.Entities;

namespace GymTracker.Application.Abstractions.Repositories;

public interface IRoutineRepository
{
	Task<Routine?> GetByIdAsync(string userId, Guid id, CancellationToken ct = default);
	Task<IReadOnlyList<Routine>> GetByUserAsync(string userId, CancellationToken ct = default);
	Task<bool> ExistsByNameAsync(string userId, string name, CancellationToken ct = default);
	Task AddAsync(Routine routine, CancellationToken ct = default);
}