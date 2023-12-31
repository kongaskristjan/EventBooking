
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

        public void CreateEvent(Event e)
        {
            _context.Events.Add(e);
            _context.SaveChanges();
        }

        public void RemoveEvent(int id)
        {
            // TODO: Remove all bookings for this event also

            // TODO: Optimize calls to the database
            var e = _context.Events.Find(id);
            _context.Events.Remove(e);
            _context.SaveChanges();
        }

        public Event[] ListEvents()
        {
            Event [] events;
            events = _context.Events.OrderBy(entity => entity.Timestamp).ToArray();
            return events;
        }

        public Event GetEvent(int id)
        {
            var e = _context.Events.Find(id);
            return e;
        }

        public void GetAllAttendees(int eventId, out Person[] persons, out Company[] companies)
        {
            persons = _context.Persons.Where(entity => entity.EventId == eventId).ToArray();
            companies = _context.Companies.Where(entity => entity.EventId == eventId).ToArray();
        }

        // Person operations

        public void CreatePerson(Person p)
        {
            _context.Persons.Add(p);
            _context.SaveChanges();
        }

        // Company operations

        public void CreateCompany(Company c)
        {
            _context.Companies.Add(c);
            _context.SaveChanges();
        }
    }
}