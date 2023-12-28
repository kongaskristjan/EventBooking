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
    private readonly ILogger<CreateEventModel> _logger;
    private readonly DbAdapter _DbAdapter;

    public ShowParticipantsModel(ILogger<CreateEventModel> logger, DbAdapter DbAdapter)
    {
        _logger = logger;
        _DbAdapter = DbAdapter;
    }

    public void OnGet()
    {
        // Show id of event
        _logger.LogInformation($"EventId: {EventId}");
    }
}
