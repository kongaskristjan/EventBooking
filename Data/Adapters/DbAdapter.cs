using Microsoft.EntityFrameworkCore;
using EventBooking.Models;

namespace EventBooking.Data.Adapters
{
    public class DbAdapter
    {
        private readonly PostgresDbContext _context;

        public DbAdapter(PostgresDbContext context)
        {
            _context = context;
        }

        // Event operations

        public async Task CreateEventAsync(Event e)
        {
            await _context.Events.AddAsync(e);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveEventAsync(int id)
        {
            // Remove all attendees
            var persons = await _context.Persons.Where(entity => entity.EventId == id).ToListAsync();
            _context.Persons.RemoveRange(persons);

            // Remove the event
            var e = await _context.Events.FindAsync(id);
            if(e != null) {
                _context.Events.Remove(e);
            } else {
                throw new KeyNotFoundException($"Event with id {id} not found");
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Event[]> ListEventsAsync()
        {
            var events = await _context.Events.OrderBy(entity => entity.Timestamp).ToArrayAsync();
            return events;
        }

        public async Task<Event> GetEventAsync(int id)
        {
            var e = await _context.Events.FindAsync(id);
            if(e == null) {
                throw new KeyNotFoundException($"Event with id {id} not found");
            }
            return e;
        }

        public async Task<(Person[], Company[])> GetAllAttendeesAsync(int eventId)
        {
            var persons = await _context.Persons.Where(entity => entity.EventId == eventId).ToArrayAsync();
            var companies = await _context.Companies.Where(entity => entity.EventId == eventId).ToArrayAsync();
            return (persons, companies);
        }

        // Person operations

        public async Task CreatePersonAsync(Person p)
        {
            await _context.Persons.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        // Company operations

        public async Task CreateCompanyAsync(Company c)
        {
            await _context.Companies.AddAsync(c);
            await _context.SaveChangesAsync();
        }
    }
}
