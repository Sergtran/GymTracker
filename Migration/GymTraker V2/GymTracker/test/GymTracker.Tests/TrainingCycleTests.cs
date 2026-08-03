using GymTracker.Domain.Entities;
using GymTracker.Domain.Exceptions;
using GymTracker.Domain.ValueObjects;

namespace GymTracker.Tests;

public class TrainingCycleTests
{
	private const string UserId = "user-1";
	private static readonly Guid RoutineId = Guid.NewGuid();

	[Fact]
	public void Create_ShouldStartAtWeekOne()
	{
		var cycle = new TrainingCycle(UserId, RoutineId);

		Assert.Equal(1, cycle.CurrentWeek);
		Assert.Equal(TrainingCycle.DefaultTotalWeeks, cycle.TotalWeeks);
		Assert.Equal(4, cycle.TotalWeeks);
		Assert.Equal(RoutineId, cycle.RoutineId);
	}

	[Fact]
	public void AdvanceWeek_ShouldIncrement()
	{
		var cycle = new TrainingCycle(UserId, RoutineId);

		cycle.AdvanceWeek();
		cycle.AdvanceWeek();

		Assert.Equal(3, cycle.CurrentWeek);
	}

	[Fact]
	public void AdvanceWeek_OnLastWeek_ShouldThrowDomainException()
	{
		var cycle = new TrainingCycle(UserId, RoutineId, totalWeeks: 4, currentWeek: 4);

		Assert.Throws<DomainException>(() => cycle.AdvanceWeek());
	}

	[Fact]
	public void Create_WithCurrentWeekOutsideRange_ShouldThrow()
	{
		Assert.Throws<ArgumentOutOfRangeException>(() => new TrainingCycle(UserId, RoutineId, currentWeek: 0));
		Assert.Throws<ArgumentOutOfRangeException>(() => new TrainingCycle(UserId, RoutineId, currentWeek: 5));
	}

	[Fact]
	public void Complete_ShouldReturnCompletedCycleWithSnapshot()
	{
		var cycle = new TrainingCycle(UserId, RoutineId, currentWeek: 3);

		var completed = cycle.Complete(new Name("Push / Pull"));

		Assert.Equal(UserId, completed.UserId);
		Assert.Equal("Push / Pull", completed.RoutineName.Value);
		Assert.Equal(3, completed.WeeksCompleted);
		Assert.True(completed.CompletedAt <= DateTime.UtcNow);
		Assert.Equal(3, cycle.CurrentWeek); // el ciclo activo no se muta
	}

	[Fact]
	public void Complete_WithEmptyRoutineName_ShouldThrow()
	{
		var cycle = new TrainingCycle(UserId, RoutineId);

		Assert.Throws<ArgumentException>(() => cycle.Complete(new Name("")));
	}

	[Fact]
	public void Create_WithEmptyUserId_ShouldThrow()
	{
		Assert.Throws<ArgumentException>(() => new TrainingCycle("", RoutineId));
	}
}
