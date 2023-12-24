using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EventBooking.Models;
using EventBooking.Data.Adapters;

namespace EventBooking.Pages;

public class IndexModel : PageModel
{
    public IEnumerable<EventWithParticipants> PastEvents { get; set; } = new List<EventWithParticipants>();
    public IEnumerable<EventWithParticipants> FutureEvents { get; set; } = new List<EventWithParticipants>();

    private readonly ILogger<IndexModel> _logger;
    private readonly DbAdapter _DbAdapter;

    public IndexModel(ILogger<IndexModel> logger, DbAdapter DbAdapter)
    {
        _logger = logger;
        _DbAdapter = DbAdapter;
    }

    public void OnGet()
    {
        // Get all events from the database
        var events = _DbAdapter.ListEvents();
        PastEvents = events.Where(e => e.Timestamp < DateTime.Now);
        FutureEvents = new List<EventWithParticipants>{};
    }
}
