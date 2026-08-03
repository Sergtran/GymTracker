using GymTracker.Domain.Common;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Domain.Entities;

/// <summary>
/// Ciclo de entrenamiento finalizado (histórico).
/// Reemplaza a previousCycle de index.html: queda registrado cuántas semanas se completaron.
/// </summary>
public sealed class CompletedTrainingCycle : Entity
{
	private CompletedTrainingCycle()
	{
		// Requerido por EF Core
	}

	public CompletedTrainingCycle(string userId, Name routineName, int weeksCompleted, DateTime completedAt)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));
		ArgumentNullException.ThrowIfNull(routineName);
		if (weeksCompleted < 1)
			throw new ArgumentOutOfRangeException(nameof(weeksCompleted), "WeeksCompleted must be at least 1.");

		Id = Guid.NewGuid();
		UserId = userId;
		RoutineName = routineName;
		WeeksCompleted = weeksCompleted;
		CompletedAt = completedAt;
	}

	public string UserId { get; private set; } = string.Empty;

	public Name RoutineName { get; private set; } = null!;

	public int WeeksCompleted { get; private set; }

	public DateTime CompletedAt { get; private set; }
}
