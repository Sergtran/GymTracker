using GymTracker.Domain.Enums;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Domain.Entities;

/// <summary>
/// Ejercicio del catálogo global (seed a partir de DEFAULT_EXERCISES de index.html).
/// Sin UserId: es compartido por todos los usuarios. El nombre es UNIQUE en BD.
/// Las preferencias individuales se guardan en <see cref="UserExercisePreference"/>.
/// El Id es int (identidad de BD) porque es dato de referencia, no un agregado del dominio
/// (los agregados siguen usando Guid). Los IDs del seed son deterministas (1..111) para HasData.
/// </summary>
public sealed class ExerciseCatalogEntry
{
	public const int MaxNameLength = 150;

	private ExerciseCatalogEntry()
	{
		// Requerido por EF Core
	}

	public ExerciseCatalogEntry(int id, Name name, ExerciseType exerciseType, Laterality defaultLaterality)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Id = id;
		Name = name;
		ExerciseType = exerciseType;
		DefaultLaterality = defaultLaterality;
	}

	public ExerciseCatalogEntry(Name name, ExerciseType exerciseType, Laterality defaultLaterality)
		: this(0, name, exerciseType, defaultLaterality)
	{
	}

	public int Id { get; private set; }

	public Name Name { get; private set; } = null!;

	public ExerciseType ExerciseType { get; private set; }

	public Laterality DefaultLaterality { get; private set; }

	#region Behaviors

	public void Rename(Name name)
	{
		ArgumentNullException.ThrowIfNull(name);
		if (name.Value.Length > MaxNameLength)
			throw new ArgumentException($"Exercise name cannot exceed {MaxNameLength} characters.", nameof(name));

		Name = name;
	}

	public void SetExerciseType(ExerciseType exerciseType)
	{
		ExerciseType = exerciseType;
	}

	public void SetDefaultLaterality(Laterality laterality)
	{
		DefaultLaterality = laterality;
	}

	#endregion
}
