using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventBooking.Data.Adapters;
using EventBooking.Models;

namespace EventBooking.Pages;

public class ShowParticipantsModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int EventId { get; set; }

    public Event CurrentEvent { get; set; } = new Event();
    public String FormattedTimestamp { get; set; }

    private readonly ILogger<CreateEventModel> _logger;
    private readonly DbAdapter _dbAdapter;

    public ShowParticipantsModel(ILogger<CreateEventModel> logger, DbAdapter dbAdapter)
    {
        _logger = logger;
        _dbAdapter = dbAdapter;
    }

    public void OnGet()
    {
        _logger.LogInformation($"ShowParticipantsModel.OnGet() called with EventId={EventId}");
        CurrentEvent = _dbAdapter.GetEvent(EventId);
        var estonianTimestamp = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(CurrentEvent.Timestamp, "FLE Standard Time");
        FormattedTimestamp = estonianTimestamp.ToString("yyyy-MM-dd HH:mm");
    }
}
