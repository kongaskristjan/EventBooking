using Microsoft.EntityFrameworkCore;
using EventBooking.Models;

public class PostgresDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Person> Persons { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
}
