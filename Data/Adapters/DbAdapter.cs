
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

        public void AddEvent(Event e)
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

        public EventWithParticipants[] ListEvents()
        {
            Event [] events;
            events = _context.Events.ToArray();

            // Convert to EventWithParticipants using a loop
            var eventsWithParticipants = new EventWithParticipants[events.Length];
            for (int i = 0; i < events.Length; i++)
            {
                eventsWithParticipants[i] = new EventWithParticipants
                {
                    Id = events[i].Id,
                    Name = events[i].Name,
                    Timestamp = events[i].Timestamp,
                    Location = events[i].Location,
                    Info = events[i].Info,
                    Participants = 0
                };
            }

            return eventsWithParticipants;
        }
    }
}