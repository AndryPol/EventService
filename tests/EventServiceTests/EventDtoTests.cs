using EventsApi.Dto;
using System.ComponentModel.DataAnnotations;

namespace EventServiceTests;

public class EventDtoTests
{
    private static EventDto ValidDto() => new()
    {
        Title = "Test",
        StartAt = new DateTime(2025, 6, 1, 10, 0, 0),
        EndAt = new DateTime(2025, 6, 1, 12, 0, 0)
    };

    private static List<ValidationResult> Validate(EventDto dto)
    {
        var context = new ValidationContext(dto);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Validate_ValidDto_NoErrors()
    {
        var results = Validate(ValidDto());
        Assert.Empty(results);
    }

    [Fact]
    public void Validate_EmptyTitle_ReturnsError()
    {
        var dto = ValidDto();
        dto.Title = "";

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.Title)));
    }

    [Fact]
    public void Validate_WhitespaceTitle_ReturnsError()
    {
        var dto = ValidDto();
        dto.Title = "   ";

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.Title)));
    }

    [Fact]
    public void Validate_TitleTooLong_ReturnsError()
    {
        var dto = ValidDto();
        dto.Title = new string('a', 31);

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.Title)));
    }

    [Fact]
    public void Validate_NullStartAt_ReturnsError()
    {
        var dto = ValidDto();
        dto.StartAt = null;

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.StartAt)));
    }

    [Fact]
    public void Validate_NullEndAt_ReturnsError()
    {
        var dto = ValidDto();
        dto.EndAt = null;

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.EndAt)));
    }

    [Fact]
    public void Validate_EndAtBeforeStartAt_ReturnsError()
    {
        var dto = ValidDto();
        dto.StartAt = new DateTime(2025, 6, 1, 12, 0, 0);
        dto.EndAt = new DateTime(2025, 6, 1, 10, 0, 0);

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.EndAt))
                                   && r.MemberNames.Contains(nameof(EventDto.StartAt)));
    }

    [Fact]
    public void Validate_EndAtEqualsStartAt_ReturnsError()
    {
        var dto = ValidDto();
        dto.StartAt = new DateTime(2025, 6, 1, 10, 0, 0);
        dto.EndAt = new DateTime(2025, 6, 1, 10, 0, 0);

        var results = Validate(dto);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(EventDto.EndAt)));
    }
}