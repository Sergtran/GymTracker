using GymTracker.Application.Abstractions;
using GymTracker.Application.Abstractions.Repositories;
using GymTracker.Application.Dtos;
using GymTracker.Application.Exceptions;
using GymTracker.Domain.Entities;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Application.Services;

public sealed class RoutineService : IRoutineService
{
	private readonly IRoutineRepository _repository;

	public RoutineService(IRoutineRepository repository)
		=> _repository = repository;

	public async Task<RoutineDto> CreateRoutineAsync(
		string userId, CreateRoutineRequest request, CancellationToken ct = default)
	{
		var name = await GetUniqueNameAsync(userId, request.Name, ct);

		var routine = new Routine(userId, new Name(name));
		await _repository.AddAsync(routine, ct);

		return new RoutineDto(routine.Id, routine.Name.Value, routine.CreatedAt);
	}

	public async Task<IReadOnlyList<RoutineDto>> GetRoutinesAsync(
		string userId, CancellationToken ct = default)
	{
		var routines = await _repository.GetByUserAsync(userId, ct);

		return routines
			.OrderBy(r => r.CreatedAt)
			.Select(r => new RoutineDto(r.Id, r.Name.Value, r.CreatedAt))
			.ToList();
	}

	public async Task<RoutineDto> GetRoutineAsync(
		string userId, Guid id, CancellationToken ct = default)
	{
		var routine = await _repository.GetByIdAsync(userId, id, ct)
			?? throw new NotFoundException("Rutina no encontrada.");

		return new RoutineDto(routine.Id, routine.Name.Value, routine.CreatedAt);
	}

	private async Task<string> GetUniqueNameAsync(
		string userId, string baseName, CancellationToken ct)
	{
		if (!await _repository.ExistsByNameAsync(userId, baseName, ct))
			return baseName;

		for (var i = 1; ; i++)
		{
			var candidate = $"{baseName} ({i})";
			if (!await _repository.ExistsByNameAsync(userId, candidate, ct))
				return candidate;
		}
	}
}