using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventBooking.Data.Adapters;
using EventBooking.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EventBooking.Pages;

public class ShowEventModel : PageModel
{
    public class FormParticipant
    {
        [Required]
        public string EntityType { get; set; } = "";

        public string? PersonFirstName { get; set; }
        public string? PersonLastName { get; set; }
        public string? PersonIdentificationNumber { get; set; }
        
        public string? CompanyName { get; set; }
        public string? CompanyRegistrationCode { get; set; }
        public int? CompanyParticipants { get; set; }

        public string PaymentMethod { get; set; } = "";
        public string? Info { get; set; } = "";
    }

    [BindProperty]
    public FormParticipant formParticipant { get; set; } = new FormParticipant();

    [BindProperty(SupportsGet = true)]
    public int EventId { get; set; }

    public Event CurrentEvent { get; set; } = new Event();
    public String FormattedTimestamp { get; set; } = "";

    public Person[] Persons { get; set; } = [];
    public Company[] Companies { get; set; } = [];

    private readonly ILogger<CreateEventModel> _logger;
    private readonly DbAdapter _dbAdapter;

    public ShowEventModel(ILogger<CreateEventModel> logger, DbAdapter dbAdapter)
    {
        _logger = logger;
        _dbAdapter = dbAdapter;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostCreateParticipantAsync()
    {
        if (!ModelState.IsValid)
        {
            await LoadDataAsync();
            return Page();
        }

        // Add the participant to the database
        if (formParticipant.EntityType == "person")
        {
            if (formParticipant.PersonFirstName == null || formParticipant.PersonLastName == null || formParticipant.PersonIdentificationNumber == null)
            {
                await LoadDataAsync();
                return Page();
            }
            var dbPerson = new Person
            {
                EventId = EventId,
                FirstName = formParticipant.PersonFirstName,
                LastName = formParticipant.PersonLastName,
                IdentificationNumber = formParticipant.PersonIdentificationNumber,
                PaymentMethod = formParticipant.PaymentMethod,
                Info = formParticipant.Info
            };
 
            _dbAdapter.CreatePerson(dbPerson);
        }
        else if (formParticipant.EntityType == "company")
        {
            if (formParticipant.CompanyName == null || formParticipant.CompanyRegistrationCode == null || formParticipant.CompanyParticipants == null)
            {
                await LoadDataAsync();
                return Page();
            }
            var dbCompany = new Company
            {
                EventId = EventId,
                Name = formParticipant.CompanyName,
                CompanyRegistrationNumber = formParticipant.CompanyRegistrationCode,
                NParticipants = (int) formParticipant.CompanyParticipants,
                PaymentMethod = formParticipant.PaymentMethod,
                Info = formParticipant.Info
            };

            _dbAdapter.CreateCompany(dbCompany);
        }
        else
        {
            throw new Exception($"Unknown entity type: {formParticipant.EntityType}");
        }

        // Reload the page
        ResetFormParticipant();
        await LoadDataAsync();
        return RedirectToPage();
    }

    private async Task LoadDataAsync()
    {
        // Show event information
        CurrentEvent = _dbAdapter.GetEvent(EventId);
        var estonianTimestamp = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(CurrentEvent.Timestamp, "FLE Standard Time");
        FormattedTimestamp = estonianTimestamp.ToString("yyyy-MM-dd HH:mm");

        // Show attendees
        Person[] persons;
        Company[] companies;
        _dbAdapter.GetAllAttendees(EventId, out persons, out companies);

        Persons = persons;
        Companies = companies;
    }

    private void ResetFormParticipant()
    {
        formParticipant.EntityType = "";
        formParticipant.PersonFirstName = null;
        formParticipant.PersonLastName = null;
        formParticipant.PersonIdentificationNumber = null;
        formParticipant.CompanyName = null;
        formParticipant.CompanyRegistrationCode = null;
        formParticipant.CompanyParticipants = null;
        formParticipant.PaymentMethod = "";
        formParticipant.Info = "";
    }
}
