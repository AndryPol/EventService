using EventsApi.EventService.Models;

namespace EventsApi.Services;

public class EventService : IEventService
{

    private readonly Dictionary<int, Event> _events = [];
    private readonly Lock _lock = new();
    private int _eventId;
    public Event? AddEvent(Event newEvent)
    { 
        lock (_lock)
        {
            if (newEvent.Id != default)
                return null;
            newEvent.Id = Interlocked.Increment(ref _eventId);
            _events.Add(newEvent.Id, newEvent);
            return newEvent;
        }
    }

    public IReadOnlyCollection<Event> GetAll()
    {
        lock (_lock)
        {
            return _events.Values
                .OrderBy(e => e.StartAt)
                .ThenBy(e => e.Id)
                .ToList()
                .AsReadOnly();
        }
    }

    public Event? GetById(int id)
    {
        lock (_lock)
        {
            if (_events.TryGetValue(id, out var result))
                return result;
            return null;
        }
    }

    public bool RemoveEvent(int id)
    {
        lock (_lock)
        {
            return _events.Remove(id);
        }
    }

    public Event? UpdateEvent(int id, Event updated)
    {
        lock (_lock)
        {
            if (!_events.TryGetValue(id, out var existing))
                return null;
            existing.Update(updated);
            return existing;
        }
    }
}
