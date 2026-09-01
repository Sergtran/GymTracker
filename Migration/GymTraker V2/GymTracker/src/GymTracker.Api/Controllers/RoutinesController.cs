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

	public RoutinesController(
		IRoutineService routineService,
		IValidator<CreateRoutineRequest> createValidator)
	{
		_routineService = routineService;
		_createValidator = createValidator;
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
	[ProducesResponseType(typeof(RoutineDto), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<RoutineDto>> GetById(Guid id, CancellationToken ct)
		=> Ok(await _routineService.GetRoutineAsync(UserId, id, ct));
}