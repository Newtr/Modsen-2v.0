using Microsoft.AspNetCore.Mvc;
using Modsen.Application;
using Modsen.Domain;

namespace Modsen.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyEventsController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MyEventsController(EventService eventService, IWebHostEnvironment hostEnvironment)
        {
            _eventService = eventService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MyEvent>>> GetAllEvents()
        {
            var allEvents = await _eventService.GetAllEventsAsync();
            return Ok(allEvents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MyEvent>> GetEventById(int id)
        {
            var myEvent = await _eventService.GetEventByIdAsync(id);
            return Ok(myEvent);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<MyEvent>> GetEventByName(string name)
        {
            var myEvent = await _eventService.GetEventByNameAsync(name);
            return Ok(myEvent);
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
