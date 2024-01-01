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

    public async Task<IActionResult> OnGetAsync()
    {
        // Populate PastEvents and FutureEvents
        var events = await _DbAdapter.ListEventsAsync();
        var now = DateTime.UtcNow;
        PastEvents = events.Where(e => e.Timestamp < now);
        FutureEvents = events.Where(e => e.Timestamp >= now);
        return Page();
    }

    public async Task<IActionResult> OnPostDeleteEventAsync(int id)
    {
        await _DbAdapter.RemoveEventAsync(id);
        return RedirectToPage("/Index");
    }
}
