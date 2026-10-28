using EventsApi.EventService.Dto;

namespace EventsApi.EventService.Models;

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
    }

    public void Update(Event updated)
    {
        Title = updated.Title;
        Description = updated.Description;
        StartAt = updated.StartAt;
        EndAt = updated.EndAt;
    }
}
