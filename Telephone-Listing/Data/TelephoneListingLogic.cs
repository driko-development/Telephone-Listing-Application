using System;
using Serilog;
using Microsoft.Extensions.Logging;
namespace Telephone_Listing.Data
{
    public class TelephoneListingLogic
    {
        public ITelephoneDataAccess DataAccess { get; set; }
        private readonly ILogger<TelephoneListingLogic> _logger;
        public TelephoneListingLogic(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TelephoneListingLogic>();
        }

        public void AddPersonToDatabase(Person person)
        {
            var success = DataAccess.AddPerson(person);

            if (success)
            {
                Console.WriteLine("Successfully added new person");
                _logger.LogInformation($"New entry created in the Person database for {person.Name}");
            }
            else
            {
                _logger.LogInformation("Unable to add the new entry to the Person table.");
                Console.WriteLine("Unable to add the new entry to the Person table.");
            }
        }

        public void DeletePersonByName(Person person)
        {
            var success = DataAccess.DeletePerson(person, searchByName: true);

            if (success)
            {
                Console.WriteLine($"Successfully deleted {person.Name}");
                _logger.LogInformation($"Entry deleted in database for {person.Name}");
            }
            else
            {
                _logger.LogInformation("Unable to delete from person table.");
                Console.WriteLine("Unable to delete from person table.");
            }
        }

        public void DeletePersonByPhoneNumber(Person person)
        {
            var success = DataAccess.DeletePerson(person, searchByName: false);

            if (success)
            {
                Console.WriteLine($"Successfully deleted entry with phone number: {person.PhoneNumber}");
                _logger.LogInformation($"Entry deleted in database for phone number: {person.PhoneNumber}");
            }
            else
            {
                _logger.LogInformation("Unable to delete from person table.");
                Console.WriteLine("Unable to delete from person table.");
            }
        }

        public void ListAllTelephoneNumbers()
        {
            string result = DataAccess.GetListing();

            if (!string.IsNullOrEmpty(result))
            {
                _logger.LogInformation("Listing all entries in the Person database.");
                Console.WriteLine(result);
            }
            else
            {
                _logger.LogInformation("Requested to list an empty database table.");
                Console.WriteLine("Add a new person to get started");
            }
        }
    }
}
