using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventBooking.Data.Adapters;
using EventBooking.Models;

namespace EventBooking.Pages;

public class CreateEventModel : PageModel
{
    public class FormEvent
    {
        [Required]
        [Display(Name = "Event name")]
        public string Name { get; set; } = "";

        [Required]
        [Display(Name = "Event date")]
        public string Date { get; set; } = "";

        [Required]
        [Display(Name = "Event location")]
        public string Location { get; set; } = "";

        [Display(Name = "Additional information")]
        public string? AdditionalInformation { get; set; }
    }

    [BindProperty]
    public FormEvent formEvent { get; set; } = new FormEvent();

    private readonly ILogger<CreateEventModel> _logger;
    private readonly DbAdapter _DbAdapter;

    public CreateEventModel(ILogger<CreateEventModel> logger, DbAdapter DbAdapter)
    {
        _logger = logger;
        _DbAdapter = DbAdapter;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostCreateEventAsync()
    {
        _logger.LogInformation("CreateEventModel.OnPostCreateEventAsync(): Name = {0}, Date = {1}, Location = {2}, AdditionalInformation = {3}", formEvent.Name, formEvent.Date, formEvent.Location, formEvent.AdditionalInformation);
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Parse the date (dd.mm.yyyy hh:mm) into a DateTimeOffset object
        var dateFormat = "dd.MM.yyyy HH:mm"; // Adjust the format to match your input
        DateTimeOffset parsedDateWithOffset;
        if (!DateTimeOffset.TryParseExact(formEvent.Date, dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDateWithOffset))
        {
            ModelState.AddModelError("Input.Date", "Invalid date format.");
            return Page();
        }

        // Convert the DateTimeOffset to Estonian time and then to UTC
        TimeZoneInfo estonianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("FLE Standard Time");
        DateTimeOffset estonianDateTimeOffset = TimeZoneInfo.ConvertTime(parsedDateWithOffset, estonianTimeZone);
        DateTime utcDateTime = estonianDateTimeOffset.UtcDateTime;

        // Add the event to the database
        var dbEvent = new Event
        {
            Name = formEvent.Name,
            Timestamp = utcDateTime,
            Location = formEvent.Location,
            Info = formEvent.AdditionalInformation ?? ""
        };
        _DbAdapter.CreateEvent(dbEvent);
        
        return RedirectToPage("/Index");
    }
}
