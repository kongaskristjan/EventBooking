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
    public String FormattedTimestamp { get; set; }

    public Person[] Persons { get; set; } = [];
    public Company[] Companies { get; set; } = [];

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

    public async Task<IActionResult> OnPostCreateParticipantAsync()
    {
        _logger.LogInformation("CreateEventModel.OnPostCreateParticipantAsync(): EntityType = {0}, PersonFirstName = {1}, PersonLastName = {2}, PersonIdentificationNumber = {3}, CompanyName = {4}, CompanyRegistrationCode = {5}, CompanyParticipants = {6}, PaymentMethod = {7}, Info = {8}", formParticipant.EntityType, formParticipant.PersonFirstName, formParticipant.PersonLastName, formParticipant.PersonIdentificationNumber, formParticipant.CompanyName, formParticipant.CompanyRegistrationCode, formParticipant.CompanyParticipants, formParticipant.PaymentMethod, formParticipant.Info);
        _logger.LogInformation($"ModelState.IsValid = {ModelState.IsValid}");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Add the participant to the database
        _logger.LogInformation($"Creating participant of type {formParticipant.EntityType}");
        if (formParticipant.EntityType == "person")
        {
            if (formParticipant.PersonFirstName == null || formParticipant.PersonLastName == null || formParticipant.PersonIdentificationNumber == null)
            {
                return Page();
            }
            _logger.LogInformation($"Creating person {formParticipant.PersonFirstName} {formParticipant.PersonLastName} with id {formParticipant.PersonIdentificationNumber}");
            var dbPerson = new Person
            {
                EventId = EventId,
                FirstName = formParticipant.PersonFirstName,
                LastName = formParticipant.PersonLastName,
                IdentificationNumber = formParticipant.PersonIdentificationNumber,
                PaymentMethod = formParticipant.PaymentMethod,
                Info = formParticipant.Info
            };
            _logger.LogInformation($"DbAdapter.CreatePerson(): Id = {dbPerson.Id}, EventId = {dbPerson.EventId}, FirstName = {dbPerson.FirstName}, LastName = {dbPerson.LastName}, IdentificationNumber = {dbPerson.IdentificationNumber}, PaymentMethod = {dbPerson.PaymentMethod}, Info = {dbPerson.Info}");
 
            _dbAdapter.CreatePerson(dbPerson);
            _logger.LogInformation($"Created person {dbPerson.FirstName} {dbPerson.LastName} with id {dbPerson.Id}");
        }
        else if (formParticipant.EntityType == "company")
        {
            if (formParticipant.CompanyName == null || formParticipant.CompanyRegistrationCode == null || formParticipant.CompanyParticipants == null)
            {
                return Page();
            }
            _logger.LogInformation($"Creating company {formParticipant.CompanyName} with registration code {formParticipant.CompanyRegistrationCode} and {formParticipant.CompanyParticipants} participants");
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
            _logger.LogInformation($"Created company {dbCompany.Name} with registration code {dbCompany.CompanyRegistrationNumber} and {dbCompany.NParticipants} participants");
        }
        else
        {
            throw new Exception($"Unknown entity type: {formParticipant.EntityType}");
        }
        
        return Page();
    }
}
