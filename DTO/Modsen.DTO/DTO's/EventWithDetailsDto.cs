namespace Modsen.DTO
{
    public class EventWithDetailsDto
    {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateOfEvent { get; set; }
    public string EventLocation { get; set; }
    public string EventCategory { get; set; }
    public int MaxMember { get; set; }
    public List<EventImageDto> EventImages { get; set; }
    public List<MemberDto> EventMembers { get; set; }
    }
}