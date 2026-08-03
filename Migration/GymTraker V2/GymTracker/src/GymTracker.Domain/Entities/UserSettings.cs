using GymTracker.Domain.Common;
using GymTracker.Domain.Enums;

namespace GymTracker.Domain.Entities;

/// <summary>
/// Preferencias del usuario: tema visual y configuración del temporizador de intervalos.
/// Un solo registro por usuario (UserId UNIQUE). Los valores por defecto del timer
/// coinciden con index.html: Prep 10s | Work 40s | Rest 20s | Sets 5.
/// </summary>
public sealed class UserSettings : Entity
{
	public static class TimerDefaults
	{
		public const int PrepSeconds = 10;
		public const int WorkSeconds = 40;
		public const int RestSeconds = 20;
		public const int Sets = 5;
	}

	private UserSettings()
	{
		// Requerido por EF Core
	}

	public UserSettings(string userId)
		: this(userId, Theme.Light, TimerDefaults.PrepSeconds, TimerDefaults.WorkSeconds, TimerDefaults.RestSeconds, TimerDefaults.Sets)
	{
	}

	public UserSettings(string userId, Theme theme, int timerPrepSeconds, int timerWorkSeconds, int timerRestSeconds, int timerSets)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("UserId cannot be empty.", nameof(userId));

		ValidateTimer(timerPrepSeconds, timerWorkSeconds, timerRestSeconds, timerSets);

		Id = Guid.NewGuid();
		UserId = userId;
		Theme = theme;
		TimerPrepSeconds = timerPrepSeconds;
		TimerWorkSeconds = timerWorkSeconds;
		TimerRestSeconds = timerRestSeconds;
		TimerSets = timerSets;
	}

	public string UserId { get; private set; } = string.Empty;

	public Theme Theme { get; private set; }

	public int TimerPrepSeconds { get; private set; }

	public int TimerWorkSeconds { get; private set; }

	public int TimerRestSeconds { get; private set; }

	public int TimerSets { get; private set; }

	#region Behaviors

	public void SetTheme(Theme theme)
	{
		Theme = theme;
	}

	public void UpdateTimer(int prepSeconds, int workSeconds, int restSeconds, int sets)
	{
		ValidateTimer(prepSeconds, workSeconds, restSeconds, sets);

		TimerPrepSeconds = prepSeconds;
		TimerWorkSeconds = workSeconds;
		TimerRestSeconds = restSeconds;
		TimerSets = sets;
	}

	#endregion

	private static void ValidateTimer(int prepSeconds, int workSeconds, int restSeconds, int sets)
	{
		if (prepSeconds < 0)
			throw new ArgumentOutOfRangeException(nameof(prepSeconds), "Prep seconds cannot be negative.");
		if (workSeconds < 1)
			throw new ArgumentOutOfRangeException(nameof(workSeconds), "Work seconds must be at least 1.");
		if (restSeconds < 1)
			throw new ArgumentOutOfRangeException(nameof(restSeconds), "Rest seconds must be at least 1.");
		if (sets < 1)
			throw new ArgumentOutOfRangeException(nameof(sets), "Sets must be at least 1.");
	}
}
