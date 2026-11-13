using EventsApi.Dto;
using EventsApi.Models;
using EventsApi.Services;

namespace EventServiceTests;

public class EventServiceTests
{
    private static EventService CreateService() => new();

    private static Event MakeEvent(string title, DateTime start, DateTime end)
    {
        var dto = new EventDto
        {
            Title = title,
            Description = "desc",
            StartAt = start,
            EndAt = end
        };
        return new Event(dto);
    }

    [Fact]
    public void AddEvent_AssignsIdAndReturnsEvent()
    {
        var service = CreateService();
        var ev = MakeEvent("Test", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0));

        var created = service.AddEvent(ev);

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("Test", created.Title);
    }

    [Fact]
    public void AddEvent_IncrementsId()
    {
        var service = CreateService();
        var first = service.AddEvent(MakeEvent("A", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));
        var second = service.AddEvent(MakeEvent("B", new DateTime(2025, 6, 2, 10, 0, 0), new DateTime(2025, 6, 2, 12, 0, 0)));

        Assert.Equal(first.Id + 1, second.Id);
    }

    [Fact]
    public void AddEvent_ReturnsClone_NotSameInstance()
    {
        var service = CreateService();
        var ev = MakeEvent("Test", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0));

        var created = service.AddEvent(ev);

        Assert.NotSame(ev, created);
        Assert.Equal(ev.Id, created.Id);
    }

    [Fact]
    public void GetById_ReturnsEvent_WhenExists()
    {
        var service = CreateService();
        var created = service.AddEvent(MakeEvent("Test", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));

        var found = service.GetById(created.Id);

        Assert.NotNull(found);
        Assert.Equal(created.Id, found.Id);
        Assert.Equal("Test", found.Title);
    }

    [Fact]
    public void GetById_ReturnsNull_WhenNotFound()
    {
        var service = CreateService();

        Assert.Null(service.GetById(999));
    }

    [Fact]
    public void UpdateEvent_UpdatesFields_KeepsId()
    {
        var service = CreateService();
        var created = service.AddEvent(MakeEvent("Old", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));

        var updated = MakeEvent("New", new DateTime(2025, 7, 1, 10, 0, 0), new DateTime(2025, 7, 1, 12, 0, 0));
        var result = service.UpdateEvent(created.Id, updated);

        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal("New", result.Title);
        Assert.Equal(new DateTime(2025, 7, 1, 10, 0, 0), result.StartAt);
    }

    [Fact]
    public void UpdateEvent_ReturnsNull_WhenNotFound()
    {
        var service = CreateService();
        var updated = MakeEvent("New", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0));

        Assert.Null(service.UpdateEvent(999, updated));
    }

    [Fact]
    public void RemoveEvent_RemovesExisting_ReturnsTrue()
    {
        var service = CreateService();
        var created = service.AddEvent(MakeEvent("Test", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));

        Assert.True(service.RemoveEvent(created.Id));
        Assert.Null(service.GetById(created.Id));
    }

    [Fact]
    public void RemoveEvent_ReturnsFalse_WhenNotFound()
    {
        var service = CreateService();

        Assert.False(service.RemoveEvent(999));
    }

    [Fact]
    public void GetEvents_NoFilters_ReturnsAllSortedByStartAt()
    {
        var service = CreateService();
        service.AddEvent(MakeEvent("B", new DateTime(2025, 6, 2, 10, 0, 0), new DateTime(2025, 6, 2, 12, 0, 0)));
        service.AddEvent(MakeEvent("A", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));

        var result = service.GetEvents(null, null, null, 1, 10);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal("A", result.Items[0].Title);
        Assert.Equal("B", result.Items[1].Title);
    }

    [Fact]
    public void GetEvents_FiltersByTitle_CaseInsensitive()
    {
        var service = CreateService();
        service.AddEvent(MakeEvent("Conference", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));
        service.AddEvent(MakeEvent("Meetup", new DateTime(2025, 6, 2, 10, 0, 0), new DateTime(2025, 6, 2, 12, 0, 0)));

        var result = service.GetEvents("conf", null, null, 1, 10);

        Assert.Single(result.Items);
        Assert.Equal("Conference", result.Items[0].Title);
    }

    [Fact]
    public void GetEvents_FiltersByFrom()
    {
        var service = CreateService();
        service.AddEvent(MakeEvent("Early", new DateTime(2025, 5, 1, 10, 0, 0), new DateTime(2025, 5, 1, 12, 0, 0)));
        service.AddEvent(MakeEvent("Late", new DateTime(2025, 7, 1, 10, 0, 0), new DateTime(2025, 7, 1, 12, 0, 0)));

        var result = service.GetEvents(null, new DateTime(2025, 6, 1), null, 1, 10);

        Assert.Single(result.Items);
        Assert.Equal("Late", result.Items[0].Title);
    }

    [Fact]
    public void GetEvents_FiltersByTo()
    {
        var service = CreateService();
        service.AddEvent(MakeEvent("Early", new DateTime(2025, 5, 1, 10, 0, 0), new DateTime(2025, 5, 1, 12, 0, 0)));
        service.AddEvent(MakeEvent("Late", new DateTime(2025, 7, 1, 10, 0, 0), new DateTime(2025, 7, 1, 12, 0, 0)));

        var result = service.GetEvents(null, null, new DateTime(2025, 6, 1), 1, 10);

        Assert.Single(result.Items);
        Assert.Equal("Early", result.Items[0].Title);
    }

    [Fact]
    public void GetEvents_CombinedFilters_LogicalAnd()
    {
        var service = CreateService();
        service.AddEvent(MakeEvent("Conference", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));
        service.AddEvent(MakeEvent("Conference Late", new DateTime(2025, 8, 1, 10, 0, 0), new DateTime(2025, 8, 1, 12, 0, 0)));
        service.AddEvent(MakeEvent("Meetup", new DateTime(2025, 6, 1, 10, 0, 0), new DateTime(2025, 6, 1, 12, 0, 0)));

        var result = service.GetEvents("conf", new DateTime(2025, 6, 1), new DateTime(2025, 7, 1), 1, 10);

        Assert.Single(result.Items);
        Assert.Equal("Conference", result.Items[0].Title);
    }

    [Fact]
    public void GetEvents_Paginates()
    {
        var service = CreateService();
        for (int i = 0; i < 5; i++)
            service.AddEvent(MakeEvent($"Event {i}", new DateTime(2025, 6, 1 + i, 10, 0, 0), new DateTime(2025, 6, 1 + i, 12, 0, 0)));

        var page1 = service.GetEvents(null, null, null, 1, 2);
        var page2 = service.GetEvents(null, null, null, 2, 2);

        Assert.Equal(5, page1.TotalCount);
        Assert.Equal(2, page1.Items.Count);
        Assert.Equal(2, page2.Items.Count);
    }

    [Fact]
    public void GetEvents_Pagination_LastPageHasRemainder()
    {
        var service = CreateService();
        for (int i = 0; i < 5; i++)
            service.AddEvent(MakeEvent($"Event {i}", new DateTime(2025, 6, 1 + i, 10, 0, 0), new DateTime(2025, 6, 1 + i, 12, 0, 0)));

        var page3 = service.GetEvents(null, null, null, 3, 2);

        Assert.Equal(5, page3.TotalCount);
        Assert.Single(page3.Items);
    }

    [Fact]
    public void GetEvents_Throws_WhenPageInvalid()
    {
        var service = CreateService();

        Assert.Throws<ArgumentException>(() => service.GetEvents(null, null, null, 0, 10));
        Assert.Throws<ArgumentException>(() => service.GetEvents(null, null, null, 1, 0));
    }
}