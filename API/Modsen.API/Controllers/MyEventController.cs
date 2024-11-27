using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.Domain;
using Modsen.DTO;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyEventsController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly IWebHostEnvironment _hostEnvironment;

        private readonly IMapper _mapper;

        public MyEventsController(EventService eventService, IWebHostEnvironment hostEnvironment, IMapper mapper)
        {
            _eventService = eventService;
            _hostEnvironment = hostEnvironment;
             _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            var allEvents = await _eventService.GetAllEventsAsync();
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(allEvents);
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(int id)
        {
            var myEvent = await _eventService.GetEventByIdAsync(id);
            var eventDto = _mapper.Map<EventDto>(myEvent);
            return Ok(eventDto);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<EventDto>> GetEventByName(string name)
        {
            var myEvent = await _eventService.GetEventByNameAsync(name);
            var eventDto = _mapper.Map<EventDto>(myEvent);
            return Ok(eventDto);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<MyEvent>>> GetEventByCriteria(DateTime? date = null, string? location = null, string? category = null)
        {
            var filteredEvents = await _eventService.GetEventsByCriteriaAsync(date, location, category);
            return Ok(filteredEvents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromForm] MyEvent newEvent, List<IFormFile> eventImages)
        {
            var createdEvent = await _eventService.CreateEventAsync(newEvent, eventImages, _hostEnvironment);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPost("{id}/add-images")]
        public async Task<IActionResult> AddImagesToEvent(int id, List<IFormFile> eventImages)
        {
            await _eventService.AddImagesToEventAsync(id, eventImages, _hostEnvironment);
            return Ok("Images successfully added.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MyEvent>> UpdateEventById(int id, [FromForm] MyEvent updatedEvent, List<IFormFile> eventImages, [FromForm] string userEmail)
        {
            await _eventService.UpdateEventAsync(id, updatedEvent, eventImages, userEmail, _hostEnvironment);
            return Ok($"Event with id {id} was updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            await _eventService.DeleteEventAsync(id, _hostEnvironment);
            return Ok($"Event with id {id} and all associated images were deleted");
        }
    }
}
