using System;

namespace Telephone_Listing.Data
{
    public class TelephoneListingLogic
    {
        public ITelephoneDataAccess DataAccess { get; set; }

        public TelephoneListingLogic()
        {

        }

        public void AddPersonToDatabase(Person person)
        {
            var success = DataAccess.AddPerson(person);

            if (success)
            {
                // log
            }
        }

        public void DeletePersonByName(Person person)
        {
            var success = DataAccess.DeletePerson(person, searchByName: true);

            if (success)
            {
                // log
            }
        }

        public void DeletePersonByPhoneNumber(Person person)
        {
            var success = DataAccess.DeletePerson(person, searchByName: false);

            if (success)
            {
                // log
            }
        }

        public void ListAllTelephoneNumbers()
        {
            Console.WriteLine(DataAccess.GetListing());
        }
    }
}
