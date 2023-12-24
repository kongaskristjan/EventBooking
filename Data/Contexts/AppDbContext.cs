using Microsoft.EntityFrameworkCore;
using EventBooking.Models;

public class PostgresDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("name=ConnectionStrings:PostgresDbContext");
    }
}
