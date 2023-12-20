using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventBooking.Pages;

public class Event
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public string Location { get; set; } = string.Empty;
    public int NParticipants { get; set; } = 0;
}

public class IndexModel : PageModel
{
    public IEnumerable<Event> PastEvents { get; set; } = new List<Event>();
    public IEnumerable<Event> FutureEvents { get; set; } = new List<Event>();

    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        PastEvents = new List<Event>
        {
            new Event
            {
                Name = "Event 1",
                Date = DateTime.Now.AddDays(-1),
                Location = "Location 1",
                NParticipants = 700
            },
        };
        FutureEvents = new List<Event>
        {
            new Event
            {
                Name = "Event 2",
                Date = DateTime.Now.AddDays(2),
                Location = "Location 2",
                NParticipants = 2
            },
            new Event
            {
                Name = "Event 3",
                Date = DateTime.Now.AddDays(3),
                Location = "Location 3",
                NParticipants = 10
            }
        };
    }
}
