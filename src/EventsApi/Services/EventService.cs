using EventsApi.Models;

namespace EventsApi.Services;

public class EventService : IEventService
{

    private readonly Dictionary<int, Event> _events = [];
    private readonly Lock _lock = new();
    private int _eventId;
    public Event AddEvent(Event newEvent)
    {
        lock (_lock)
        {
            newEvent.Id = Interlocked.Increment(ref _eventId);
            _events.Add(newEvent.Id, newEvent);
            return newEvent.Clone();
        }
    }

    public IReadOnlyCollection<Event> GetEvents(string? title, DateTime? from, DateTime? to)
    {
        lock (_lock)
        {
            IEnumerable<Event> query = _events.Values;

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (from.HasValue)
                query = query.Where(e => e.StartAt >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.EndAt <= to.Value);

            return query
                .OrderBy(e => e.StartAt)
                .ThenBy(e => e.Id)
                .Select(e => e.Clone())
                .ToList()
                .AsReadOnly();
        }
    }

    public Event? GetById(int id)
    {
        lock (_lock)
        {
            if (_events.TryGetValue(id, out var result))
                return result.Clone();
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
            return existing.Clone();
        }
    }
}
