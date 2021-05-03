using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Telephone_Listing.Data;

namespace Telephone_Listing.Services
{
    public enum Command { ADD, DEL, LIST }
    
    public class ListingService
    {
        private readonly IConfigurationRoot _config;
        private TelephoneListingLogic _telephoneListingLogic;
        
        public ListingService(IConfigurationRoot config)
        {
            _config = config;
            _telephoneListingLogic = new TelephoneListingLogic();
            _telephoneListingLogic.DataAccess = new TelephoneDataAccess(config);
        }

        public async Task Run(Command command, Person person)
        {
            switch (command)
            {
                case Command.ADD:
                    _telephoneListingLogic.AddPersonToDatabase(person);
                    break;
                case Command.DEL:
                    if (!string.IsNullOrEmpty(person.PhoneNumber))
                        _telephoneListingLogic.DeletePersonByPhoneNumber(person);
                    else
                        _telephoneListingLogic.DeletePersonByName(person);
                    break;
                case Command.LIST:
                    _telephoneListingLogic.ListAllTelephoneNumbers();
                    break;
            }
        }
    }
}
