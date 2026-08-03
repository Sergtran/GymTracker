using GymTracker.Domain.Common;
using GymTracker.Domain.Exceptions;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Domain.Entities;

/// <summary>
/// Ciclo de entrenamiento activo (4 semanas por defecto, igual que index.html).
/// Reglas: máximo un ciclo activo por usuario (UNIQUE en UserId) y
/// CurrentWeek siempre entre 1 y TotalWeeks. RoutineId es Guid: nunca se usa un índice
/// de posición que se rompa al borrar/reordenar rutinas (ver documentación 5.3).
/// </summary>
public sealed class TrainingCycle : Entity
{
	public const int DefaultTotalWeeks = 4;

	private TrainingCycle()
	{
		// Requerido por EF Core
	}

	public TrainingCycle(string userId, Guid routineId, int totalWeeks = DefaultTotalWeeks, int? currentWeek = null)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));
		if (totalWeeks < 1)
			throw new ArgumentOutOfRangeException(nameof(totalWeeks), "TotalWeeks must be at least 1.");

		var week = currentWeek ?? 1;
		if (week < 1 || week > totalWeeks)
			throw new ArgumentOutOfRangeException(nameof(currentWeek), $"CurrentWeek must be between 1 and {totalWeeks}.");

		Id = Guid.NewGuid();
		UserId = userId;
		RoutineId = routineId;
		CurrentWeek = week;
		TotalWeeks = totalWeeks;
		StartedAt = DateTime.UtcNow;
	}

	public string UserId { get; private set; } = string.Empty;

	public Guid RoutineId { get; private set; }

	public int CurrentWeek { get; private set; }

	public int TotalWeeks { get; private set; }

	public DateTime StartedAt { get; private set; }

	#region Behaviors

	/// <summary>
	/// Avanza a la siguiente semana. Si el ciclo ya está en la última semana,
	/// lanza <see cref="DomainException"/>: ahí el caso de uso debe completar el ciclo.
	/// </summary>
	public void AdvanceWeek()
	{
		if (CurrentWeek >= TotalWeeks)
			throw new DomainException($"Cannot advance past week {TotalWeeks}. Complete the cycle instead.");

		CurrentWeek++;
	}

	/// <summary>
	/// Cierra el ciclo activo y produce el registro histórico.
	/// (El caso de uso es responsable de persistir el <see cref="CompletedTrainingCycle"/>
	/// y eliminar el ciclo activo.)
	/// </summary>
	public CompletedTrainingCycle Complete(Name routineName)
	{
		ArgumentNullException.ThrowIfNull(routineName);

		return new CompletedTrainingCycle(UserId, routineName, CurrentWeek, DateTime.UtcNow);
	}

	#endregion
}
