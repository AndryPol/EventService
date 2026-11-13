using EventsApi.Models;

namespace EventsApi.Services;

public interface IEventService
{
    IReadOnlyCollection<Event> GetEvents(string? title, DateTime? from, DateTime? to);
    Event? GetById(int id);
    Event AddEvent(Event newEvent);
    Event? UpdateEvent(int id, Event updated);
    bool RemoveEvent(int id);
}
