using EventsApi.Dto;
using EventsApi.Models;

namespace EventServiceTests;

public class EventTests
{
    private static EventDto ValidDto() => new()
    {
        Title = "Test",
        Description = "desc",
        StartAt = new DateTime(2025, 6, 1, 10, 0, 0),
        EndAt = new DateTime(2025, 6, 1, 12, 0, 0)
    };

    [Fact]
    public void Constructor_Throws_WhenEndAtBeforeStartAt()
    {
        var dto = ValidDto();
        dto.StartAt = new DateTime(2025, 6, 1, 12, 0, 0);
        dto.EndAt = new DateTime(2025, 6, 1, 10, 0, 0);

        Assert.Throws<ArgumentException>(() => new Event(dto));
    }

    [Fact]
    public void Constructor_Throws_WhenEndAtEqualsStartAt()
    {
        var dto = ValidDto();
        dto.StartAt = new DateTime(2025, 6, 1, 10, 0, 0);
        dto.EndAt = new DateTime(2025, 6, 1, 10, 0, 0);

        Assert.Throws<ArgumentException>(() => new Event(dto));
    }

    [Fact]
    public void Constructor_CopiesFieldsFromDto()
    {
        var dto = ValidDto();
        var ev = new Event(dto);

        Assert.Equal(dto.Title, ev.Title);
        Assert.Equal(dto.Description, ev.Description);
        Assert.Equal(dto.StartAt, ev.StartAt);
        Assert.Equal(dto.EndAt, ev.EndAt);
    }

    [Fact]
    public void Clone_CreatesIndependentCopy()
    {
        var ev = new Event(ValidDto());
        ev.Id = 42;

        var clone = ev.Clone();

        Assert.NotSame(ev, clone);
        Assert.Equal(ev.Id, clone.Id);
        Assert.Equal(ev.Title, clone.Title);
        Assert.Equal(ev.StartAt, clone.StartAt);
    }

    [Fact]
    public void Clone_ModificationDoesNotAffectOriginal()
    {
        var ev = new Event(ValidDto());
        ev.Id = 1;

        var clone = ev.Clone();
        clone.Title = "Changed";

        Assert.Equal("Test", ev.Title);
        Assert.Equal("Changed", clone.Title);
    }
}