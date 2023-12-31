using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventBooking.Data.Adapters;
using EventBooking.Models;

namespace EventBooking.Pages;

public class ShowParticipantsModel : PageModel
{
    public class FormParticipant
    {
        [Required]
        public string EntityType { get; set; } = "";

        public string PersonFirstName { get; set; } = "";
        public string PersonLastName { get; set; } = "";
        public string PersonIdentificationNumber { get; set; } = "";
        
        public string CompanyName { get; set; } = "";
        public string CompanyRegistrationCode { get; set; } = "";
        public int CompanyParticipants { get; set; } = 1;

        [Required]
        public string PaymentMethod { get; set; } = "";
        public string? Info { get; set; }
    }

    [BindProperty]
    public FormParticipant formParticipant { get; set; } = new FormParticipant();

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

    public async Task<IActionResult> OnPostCreateParticipantAsync()
    {
        _logger.LogInformation("CreateEventModel.OnPostCreateParticipantAsync(): EntityType = {0}, PersonFirstName = {1}, PersonLastName = {2}, PersonIdentificationNumber = {3}, CompanyName = {4}, CompanyRegistrationCode = {5}, CompanyParticipants = {6}, PaymentMethod = {7}, Info = {8}", formParticipant.EntityType, formParticipant.PersonFirstName, formParticipant.PersonLastName, formParticipant.PersonIdentificationNumber, formParticipant.CompanyName, formParticipant.CompanyRegistrationCode, formParticipant.CompanyParticipants, formParticipant.PaymentMethod, formParticipant.Info);
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Add the participant to the database
        /*
        var dbEvent = new Event
        {
            Name = formEvent.Name,
            Timestamp = utcDateTime,
            Location = formEvent.Location,
            Info = formEvent.AdditionalInformation ?? ""
        };
        _DbAdapter.CreateEvent(dbEvent);
        */
        
        return RedirectToPage("/Index");
    }
}
