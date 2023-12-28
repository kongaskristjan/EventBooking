using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using EventBooking.Models;
using EventBooking.Data.Adapters;

namespace EventBooking.Pages;

public class IndexModel : PageModel
{
    public IEnumerable<Event> PastEvents { get; set; } = new List<Event>();
    public IEnumerable<Event> FutureEvents { get; set; } = new List<Event>();

    private readonly ILogger<IndexModel> _logger;
    private readonly DbAdapter _DbAdapter;

    public IndexModel(ILogger<IndexModel> logger, DbAdapter DbAdapter)
    {
        _logger = logger;
        _DbAdapter = DbAdapter;
    }

    public void OnGet()
    {
        // Populate PastEvents and FutureEvents
        var events = _DbAdapter.ListEvents();
        var now = DateTime.UtcNow;
        PastEvents = events.Where(e => e.Timestamp < now);
        FutureEvents = events.Where(e => e.Timestamp >= now);
    }

    public async Task<IActionResult> OnPostDeleteEventAsync(int id)
    {
        _DbAdapter.RemoveEvent(id);
        return RedirectToPage("/Index");
    }
}
