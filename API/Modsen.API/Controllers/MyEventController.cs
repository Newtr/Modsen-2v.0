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
        private readonly GetEventsUseCase _getEventsUseCase;
        private readonly CreateEventUseCase _createEventUseCase;
        private readonly UpdateEventUseCase _updateEventUseCase;
        private readonly DeleteEventUseCase _deleteEventUseCase;
        private readonly AddImagesToEventUseCase _addImagesToEventUseCase;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMapper _mapper;

        public MyEventsController(
            GetEventsUseCase getEventsUseCase,
            CreateEventUseCase createEventUseCase,
            UpdateEventUseCase updateEventUseCase,
            DeleteEventUseCase deleteEventUseCase,
            AddImagesToEventUseCase addImagesToEventUseCase,
            IWebHostEnvironment hostEnvironment,
            IMapper mapper)
        {
            _getEventsUseCase = getEventsUseCase;
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _addImagesToEventUseCase = addImagesToEventUseCase;
            _hostEnvironment = hostEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents(CancellationToken cancellationToken)
        {
            var allEvents = await _getEventsUseCase.GetAllEventsAsync(cancellationToken);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(allEvents);
            return Ok(eventDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEventById(int id, CancellationToken cancellationToken)
        {
            var myEvent = await _getEventsUseCase.GetEventByIdAsync(id, cancellationToken);
            var eventDto = _mapper.Map<EventDto>(myEvent);
            return Ok(eventDto);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<EventDto>> GetEventByName(string name, CancellationToken cancellationToken)
        {
            var myEvent = await _getEventsUseCase.GetEventByNameAsync(name, cancellationToken);
            var eventDto = _mapper.Map<EventDto>(myEvent);
            return Ok(eventDto);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<MyEvent>>> GetEventByCriteria(DateTime? date = null, string? location = null, string? category = null, CancellationToken cancellationToken = default)
        {
            var filteredEvents = await _getEventsUseCase.GetEventsByCriteriaAsync(date, location, category, cancellationToken);
            return Ok(filteredEvents);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromForm] MyEvent newEvent, List<IFormFile> eventImages, CancellationToken cancellationToken)
        {
            var createdEvent = await _createEventUseCase.CreateEventAsync(newEvent, eventImages, _hostEnvironment, cancellationToken);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpPost("{id}/add-images")]
        public async Task<IActionResult> AddImagesToEvent(int id, List<IFormFile> eventImages, CancellationToken cancellationToken)
        {
            await _addImagesToEventUseCase.AddImagesToEventAsync(id, eventImages, _hostEnvironment, cancellationToken);
            return Ok("Images successfully added.");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MyEvent>> UpdateEventById(int id, [FromForm] MyEvent updatedEvent, List<IFormFile> eventImages, [FromForm] string userEmail, CancellationToken cancellationToken)
        {
            await _updateEventUseCase.UpdateEventAsync(id, updatedEvent, eventImages, userEmail, _hostEnvironment, cancellationToken);
            return Ok($"Event with id {id} was updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            await _deleteEventUseCase.DeleteEventAsync(id, _hostEnvironment, cancellationToken);
            return Ok($"Event with id {id} and all associated images were deleted");
        }
    }
}
