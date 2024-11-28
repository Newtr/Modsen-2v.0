using Microsoft.EntityFrameworkCore;
using Modsen.Domain;
using Modsen.Infrastructure;
using Xunit;

namespace Modsen.Tests
{
    public class UserApiTests
    {
        private readonly ModsenContext _context;

        public UserApiTests()
        {
            var options = new DbContextOptionsBuilder<ModsenContext>()
                .UseInMemoryDatabase(databaseName: "ModsenTestDb")
                .Options;

            _context = new ModsenContext(options);
        }

        [Fact]
        public async Task CreateUser_ShouldAddUser()
        {
            var user = new User { Email = "test@example.com", RefreshToken = "token123", RoleId = 1 };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var retrievedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

            Assert.NotNull(retrievedUser);
            Assert.Equal(user.Email, retrievedUser.Email);
        }

        [Fact]
        public async Task CreateMember_ShouldAddMember()
        {
            var member = new Member { Name = "John Doe", Email = "johndoe@example.com" };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            var retrievedMember = await _context.Members.FirstOrDefaultAsync(m => m.Email == member.Email);

            Assert.NotNull(retrievedMember);
            Assert.Equal(member.Name, retrievedMember.Name);
            Assert.Equal(member.Email, retrievedMember.Email);
        }

        [Fact]
        public async Task CreateEvent_ShouldAddEvent()
        {
            var myEvent = new MyEvent { Name = "Conference 2024", Description = "Annual conference", DateOfEvent = DateTime.Now };

            _context.Events.Add(myEvent);
            await _context.SaveChangesAsync();

            var retrievedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Name == myEvent.Name);

            Assert.NotNull(retrievedEvent);
            Assert.Equal(myEvent.Name, retrievedEvent.Name);
            Assert.Equal(myEvent.Description, retrievedEvent.Description);
        }

        [Fact]
        public async Task SubscribeMemberToEvent_ShouldWork()
        {
            var member = new Member { Name = "Jane Doe", Email = "janedoe@example.com" };
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            var myEvent = new MyEvent { Name = "Tech Talk", Description = "Discussion on technology", DateOfEvent = DateTime.Now };
            _context.Events.Add(myEvent);
            await _context.SaveChangesAsync();

            member.MemberEvents.Add(myEvent);
            await _context.SaveChangesAsync();

            var subscribedEvent = await _context.Events
                .Include(e => e.EventMembers)
                .FirstOrDefaultAsync(e => e.Id == myEvent.Id);

            Assert.NotNull(subscribedEvent);
            Assert.Contains(subscribedEvent.EventMembers, m => m.Id == member.Id);
        }

        [Fact]
        public async Task GetAllMembers_ShouldReturnListOfMembers()
        {
            var member1 = new Member { Name = "John Doe", Email = "john.doe@example.com" };
            var member2 = new Member { Name = "Jane Doe", Email = "jane.doe@example.com" };

            _context.Members.AddRange(member1, member2);
            await _context.SaveChangesAsync();

            var members = await _context.Members.ToListAsync();

            Assert.NotNull(members);
            Assert.True(members.Count >= 2);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnListOfEvents()
        {
            var event1 = new MyEvent { Name = "Conference 2024", Description = "Annual conference", DateOfEvent = DateTime.Now };
            var event2 = new MyEvent { Name = "Workshop 2024", Description = "Annual workshop", DateOfEvent = DateTime.Now };

            _context.Events.AddRange(event1, event2);
            await _context.SaveChangesAsync();

            var events = await _context.Events.ToListAsync();

            Assert.NotNull(events);
            Assert.True(events.Count >= 2);
        }
    }
}
