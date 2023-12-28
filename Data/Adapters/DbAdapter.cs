
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
    }
}