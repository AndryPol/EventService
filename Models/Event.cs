using EventsApi.Dto;

namespace EventsApi.Models;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }

    public Event(EventDto dto)
    {
        Title = dto.Title!;
        Description = dto.Description;
        StartAt = dto.StartAt!.Value;
        EndAt = dto.EndAt!.Value;
        if (EndAt <= StartAt)
            throw new ArgumentException("EndAt must be later than StartAt.");
    }

    public void Update(Event updated)
    {
        if (updated.EndAt <= updated.StartAt)
            throw new ArgumentException("EndAt must be later than StartAt.");
        Title = updated.Title;
        Description = updated.Description;
        StartAt = updated.StartAt;
        EndAt = updated.EndAt;
    }

    public Event Clone()
    {
        return new Event(this);
    }

    private Event(Event other)
    {
        Id = other.Id;
        Title = other.Title;
        Description = other.Description;
        StartAt = other.StartAt;
        EndAt = other.EndAt;
    }
}
