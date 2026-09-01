using GymTracker.Application.Dtos;

namespace GymTracker.Application.Abstractions;

public interface IRoutineService
{
	Task<RoutineDto> CreateRoutineAsync(string userId, CreateRoutineRequest request, CancellationToken ct = default);
	Task<IReadOnlyList<RoutineDto>> GetRoutinesAsync(string userId, CancellationToken ct = default);
	Task<RoutineDto> GetRoutineAsync(string userId, Guid id, CancellationToken ct = default);
}