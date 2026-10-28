using EventsApi.EventService.Dto;
using EventsApi.EventService.Models;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;


namespace EventsApi.Controllers;

[Route("events")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EventDto>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<EventDto>> GetAllEvents()
    {
        var events = _eventService.GetAll()
            .Select(e => new EventDto(e))
            .ToList();

        return Ok(events);
    }
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EventDto> GetEventById(int id)
    {
        var ev = _eventService.GetById(id);
        return ev is null
            ? NotFound()
            : Ok(new EventDto(ev));
    }

    [HttpPost]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<EventDto> CreateEvent([FromBody] EventDto dto)
    {
        var created = _eventService.AddEvent(new Event(dto));

        if (created is null)
            return BadRequest();

        var response = new EventDto(created);
        return CreatedAtAction(nameof(GetEventById), new { id = response.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EventDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<EventDto> UpdateEvent(int id, [FromBody] EventDto dto)
    {
        var updated = _eventService.UpdateEvent(id, new Event(dto));

        return updated is null
            ? NotFound()
            : Ok(new EventDto(updated));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteEvent(int id)
    {
        return _eventService.RemoveEvent(id) ? Ok() : NotFound();
    }
}
