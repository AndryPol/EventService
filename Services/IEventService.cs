using EventsApi.EventService.Models;

namespace EventsApi.Services;

public interface IEventService
{
    IReadOnlyCollection<Event> GetAll();
    Event? GetById(int id);
    Event? AddEvent(Event newEvent);
    Event? UpdateEvent(int id, Event updated);
    bool RemoveEvent(int id);
}
