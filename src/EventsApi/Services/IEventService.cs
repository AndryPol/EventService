using EventsApi.Dto;
using EventsApi.Models;

namespace EventsApi.Services;

public interface IEventService
{
    PaginatedResult<Event> GetEvents(string? title, DateTime? from, DateTime? to, int page, int pageSize);
    Event? GetById(int id);
    Event AddEvent(Event newEvent);
    Event? UpdateEvent(int id, Event updated);
    bool RemoveEvent(int id);
}
