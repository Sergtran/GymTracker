using System.Security.Claims;
using GymTracker.Application.Abstractions;
using GymTracker.Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppValidationException = GymTracker.Application.Exceptions.ValidationException;

namespace GymTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RoutinesController : ControllerBase
{
	private readonly IRoutineService _routineService;
	private readonly IValidator<CreateRoutineRequest> _createValidator;
	private readonly IValidator<CreateSessionRequest> _sessionValidator;
	private readonly IValidator<CreateSessionExerciseRequest> _exerciseValidator;

	public RoutinesController(
		IRoutineService routineService,
		IValidator<CreateRoutineRequest> createValidator,
		IValidator<CreateSessionRequest> sessionValidator,
		IValidator<CreateSessionExerciseRequest> exerciseValidator)
	{
		_routineService = routineService;
		_createValidator = createValidator;
		_sessionValidator = sessionValidator;
		_exerciseValidator = exerciseValidator;
	}

	private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
		?? throw new InvalidOperationException("User id claim not found.");

	[HttpPost]
	[ProducesResponseType(typeof(RoutineDto), StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<RoutineDto>> Create(CreateRoutineRequest request, CancellationToken ct)
	{
		var validation = await _createValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		var routine = await _routineService.CreateRoutineAsync(UserId, request, ct);
		return CreatedAtAction(nameof(GetById), new { id = routine.Id }, routine);
	}

	[HttpGet]
	[ProducesResponseType(typeof(IReadOnlyList<RoutineDto>), StatusCodes.Status200OK)]
	public async Task<ActionResult<IReadOnlyList<RoutineDto>>> GetAll(CancellationToken ct)
		=> Ok(await _routineService.GetRoutinesAsync(UserId, ct));

	[HttpGet("{id:guid}")]
	[ProducesResponseType(typeof(RoutineDetailDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<RoutineDetailDto>> GetById(Guid id, CancellationToken ct)
	=> Ok(await _routineService.GetRoutineDetailAsync(UserId, id, ct));

	[HttpPost("{routineId:guid}/sessions")]
	public async Task<ActionResult<SessionDto>> AddSession(
	Guid routineId, CreateSessionRequest request, CancellationToken ct)
	{
		var validation = await _sessionValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _routineService.AddSessionAsync(UserId, routineId, request, ct));
	}

	[HttpPost("{routineId:guid}/sessions/{sessionId:guid}/exercises")]
	public async Task<ActionResult<SessionExerciseDto>> AddExercise(
		Guid routineId, Guid sessionId, CreateSessionExerciseRequest request, CancellationToken ct)
	{
		var validation = await _exerciseValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _routineService.AddExerciseAsync(UserId, routineId, sessionId, request, ct));
	}

	[HttpPut("{id:guid}")]
	[ProducesResponseType(typeof(RoutineDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<RoutineDto>> Rename(Guid id, CreateRoutineRequest request, CancellationToken ct)
	{
		var validation = await _createValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _routineService.RenameRoutineAsync(UserId, id, request, ct));
	}

	[HttpDelete("{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
	{
		await _routineService.DeleteRoutineAsync(UserId, id, ct);
		return NoContent();
	}

	[HttpPut("{routineId:guid}/sessions/{sessionId:guid}")]
	[ProducesResponseType(typeof(SessionDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<SessionDto>> RenameSession(
		Guid routineId, Guid sessionId, CreateSessionRequest request, CancellationToken ct)
	{
		var validation = await _sessionValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _routineService.RenameSessionAsync(UserId, routineId, sessionId, request, ct));
	}

	[HttpDelete("{routineId:guid}/sessions/{sessionId:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteSession(Guid routineId, Guid sessionId, CancellationToken ct)
	{
		await _routineService.DeleteSessionAsync(UserId, routineId, sessionId, ct);
		return NoContent();
	}

	[HttpPut("{routineId:guid}/sessions/{sessionId:guid}/exercises/{exerciseId:guid}")]
	[ProducesResponseType(typeof(SessionExerciseDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<SessionExerciseDto>> EditExercise(
		Guid routineId, Guid sessionId, Guid exerciseId,
		CreateSessionExerciseRequest request, CancellationToken ct)
	{
		var validation = await _exerciseValidator.ValidateAsync(request, ct);
		if (!validation.IsValid)
			throw new AppValidationException(validation.Errors.Select(e => e.ErrorMessage));

		return Ok(await _routineService.EditExerciseAsync(UserId, routineId, sessionId, exerciseId, request, ct));
	}

	[HttpDelete("{routineId:guid}/sessions/{sessionId:guid}/exercises/{exerciseId:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteExercise(
		Guid routineId, Guid sessionId, Guid exerciseId, CancellationToken ct)
	{
		await _routineService.DeleteExerciseAsync(UserId, routineId, sessionId, exerciseId, ct);
		return NoContent();
	}
}
