using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Telephone_Listing.Data
{
    public class TelephoneDataAccess : ITelephoneDataAccess
    {
        private readonly IConfigurationRoot _config;
        private SqlConnection _sqlConnection;

        public TelephoneDataAccess(IConfigurationRoot configuration)
        {
            _config = configuration;
            SetUpDatabaseConnection();
        }

        private void SetUpDatabaseConnection()
        {
            var connString = _config.GetConnectionString("");
            _sqlConnection = new SqlConnection(connString);
        }

        public bool AddPerson(Person person)
        {
            var sql = "INSERT INTO Person " +
                      "VALUES (@Name, @PhoneNumber);";

            int rows_affected;
            using (_sqlConnection)
            {
                var cmd = new SqlCommand(sql, _sqlConnection);
                cmd.Parameters.AddWithValue("@Name", person.Name);
                cmd.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber);

                cmd.Connection.Open();
                rows_affected = cmd.ExecuteNonQuery();
            }

            if (rows_affected > 1) return true;
            else return false;
        }

        public bool DeletePerson(Person person, bool searchByName=true)
        {
            var sql = string.Concat(
                    "DELETE FROM Person ",
                    searchByName ? "WHERE Name=@Name;"
                                 : "WHERE PhoneNumber=@PhoneNumber;"
                );

            int rows_affected;
            using (_sqlConnection)
            {
                var cmd = new SqlCommand(sql, _sqlConnection);
                if (searchByName)
                {
                    cmd.Parameters.AddWithValue("@Name", person.Name);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@PhoneNumber", person.PhoneNumber);
                }

                cmd.Connection.Open();
                rows_affected = cmd.ExecuteNonQuery();
            }

            if (rows_affected > 1) return true;
            else return false;
        }

        public string GetListing()
        {
            DataTable telephoneListing = new DataTable();

            var sql = "SELECT * " +
                      "FROM Person;";
            using (_sqlConnection)
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sql, _sqlConnection);
                sqlDataAdapter.SelectCommand.Connection.Open();

                sqlDataAdapter.Fill(telephoneListing);
            }

            return PrettyPrintTelephones(telephoneListing);
        }

        private string PrettyPrintTelephones(DataTable telephoneListing)
        {
            return string.Join(Environment.NewLine,
                telephoneListing.Rows.OfType<DataRow>().Select(x => string.Join(" ; ", x.ItemArray)));
        }
    }
}
