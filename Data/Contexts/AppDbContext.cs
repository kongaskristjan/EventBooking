using Microsoft.EntityFrameworkCore;
using EventBooking.Models;

public class PostgresDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
    }
}
