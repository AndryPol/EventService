using EventsApi.EventService.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EventsApi.EventService.Dto;

public class EventDto : IValidatableObject
{
    public int Id { get; private set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartAt { get; set; }
    public DateTime? EndAt { get; set; }

    [JsonConstructor]
    public EventDto() { }

    public EventDto(Event model)
    {
        Id = model.Id;
        Title = model.Title;
        Description = model.Description;
        StartAt = model.StartAt;
        EndAt = model.EndAt;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Title))
        {
            yield return new ValidationResult(
                "Title is required and must not be whitespace.",
                [nameof(Title)]);
        }
        else if (Title.Length > 30)
        {
            yield return new ValidationResult(
                "Title must not exceed 30 characters.",
                [nameof(Title)]);
        }


        if (!StartAt.HasValue)
        {
            yield return new ValidationResult(
                "StartAt is required.",
                [nameof(StartAt)]);
        }


        if (!EndAt.HasValue)
        {
            yield return new ValidationResult(
                "EndAt is required.",
                [nameof(EndAt)]);
        }
        if (StartAt.HasValue && EndAt.HasValue && EndAt.Value <= StartAt.Value)
        {
            yield return new ValidationResult(
                "EndAt must be later than StartAt.",
                [nameof(EndAt), nameof(StartAt)]);
        }
    }
}
