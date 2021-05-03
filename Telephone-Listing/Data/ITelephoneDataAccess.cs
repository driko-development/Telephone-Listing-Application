namespace Telephone_Listing.Data
{
    public interface ITelephoneDataAccess
    {
        string GetListing();
        bool AddPerson(Person person);
        bool DeletePerson(Person person, bool searchByName = true);
    }
}
