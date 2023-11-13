using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Capstone.Pages.DB
{
    public class DBClass
    {
        public static SqlConnection CapDBConn = new SqlConnection();
        public static readonly String CapDBConnString = "Server = Localhost;Database = Cap;Trusted_Connection = True;TrustServerCertificate=true;";
        private static readonly String? AuthConnString = "Server=Localhost;Database=AUTH;Trusted_Connection=True;TrustServerCertificate=True";
        public void InsertEvent(Event eventModel)
        {
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Event (Name, Description, Address, StartDate, EndDate, RegistrationCost) 
                                VALUES (@Name, @Description, @Address, @StartDate, @EndDate, @RegistrationCost)";

            command.Parameters.AddWithValue("@Name", eventModel.Name);
            command.Parameters.AddWithValue("@Description", eventModel.Description);
            command.Parameters.AddWithValue("@Address", eventModel.Address);
            command.Parameters.AddWithValue("@StartDate", eventModel.StartDate);
            command.Parameters.AddWithValue("@EndDate", eventModel.EndDate);
            command.Parameters.AddWithValue("@RegistrationCost", eventModel.RegistrationCost);
            command.Parameters.AddWithValue("@EventType", eventModel.EventType);
            command.Parameters.AddWithValue("@EstimatedAttendance", eventModel.EstimatedAttendance);

            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }




        public static void InsertRequestedEvent(Event eventModel)
        {
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO RequestedEvent (Name, Description, Address, StartDate, EndDate, RegistrationCost, EventType, EstimatedAttendance) 
                                VALUES (@Name, @Description, @Address, @StartDate, @EndDate, @RegistrationCost, @EventType, @EstimatedAttendance)";

            command.Parameters.AddWithValue("@Name", eventModel.Name);
            command.Parameters.AddWithValue("@Description", eventModel.Description);
            command.Parameters.AddWithValue("@Address", eventModel.Address);
            command.Parameters.AddWithValue("@StartDate", eventModel.StartDate);
            command.Parameters.AddWithValue("@EndDate", eventModel.EndDate);
            command.Parameters.AddWithValue("@RegistrationCost", eventModel.RegistrationCost);
            command.Parameters.AddWithValue("@EventType", eventModel.EventType);
            command.Parameters.AddWithValue("@EstimatedAttendance", eventModel.EstimatedAttendance);

            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }



        public static int LoginQuery(string loginQuery)
        {
            // This method expects to receive an SQL SELECT
            // query that uses the COUNT command.
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = CapDBConn;
            cmdLogin.Connection.ConnectionString = CapDBConnString;
            cmdLogin.CommandText = loginQuery;
            cmdLogin.Connection.Open();
            // ExecuteScalar() returns back data type Object
            // Use a typecast to convert this to an int.
            // Method returns first column of first row.
            int rowCount = (int)cmdLogin.ExecuteScalar();
            return rowCount;
        }



        public static void CreateHashedUser(string Username, string Password)
        {
            string loginQuery = "Insert into HashedCredentials (Username, Password) values (@Username, @Password)";
            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = CapDBConn;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));

            cmdLogin.Connection.Open();

            cmdLogin.ExecuteNonQuery();
        }

        public static bool HashedParameterLogin(string Username, string Password)
        {
            string loginQuery = "SELECT * from HashedCredentials Where Username = @Username";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = CapDBConn;
            cmdLogin.Connection.ConnectionString = AuthConnString;

            cmdLogin.CommandText = loginQuery;
            cmdLogin.Parameters.AddWithValue("@Username", Username);

            cmdLogin.Connection.Open();

            SqlDataReader hashReader = cmdLogin.ExecuteReader();

            if (hashReader.Read())
            {
                string correctHash = hashReader["Password"].ToString();

                if (PasswordHash.ValidatePassword(Password, correctHash))
                {

                    return true;
                }
            }
            return false;
        }

        public static bool StoredProcedureLogin(string Username, string Password)
        {
            SqlCommand cmdProductRead = new SqlCommand();
            cmdProductRead.Connection = new SqlConnection();
            cmdProductRead.Connection.ConnectionString = AuthConnString;
            cmdProductRead.CommandType = System.Data.CommandType.StoredProcedure;
            cmdProductRead.Parameters.AddWithValue("@Username", Username);
            cmdProductRead.Parameters.AddWithValue("@Password", Password);
            cmdProductRead.CommandText = "sp_Capstone";
            cmdProductRead.Connection.Open();
            if (((int)cmdProductRead.ExecuteScalar()) > 0)
            {
                return true;
            }
            return false;
        }


        public static SqlDataReader GetUserRoleByUsername(string Username)
        {
            string roleQuery = "SELECT UserType FROM EventUser Where Username = @Username";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = CapDBConn;
            cmdLogin.Connection.ConnectionString = CapDBConnString;
            cmdLogin.Parameters.AddWithValue("@Username", Username);
            cmdLogin.CommandText = roleQuery;
            cmdLogin.Connection.Open();
            SqlDataReader tempReader = cmdLogin.ExecuteReader();
            return tempReader;
        }

        public static SqlDataReader SearchEvent(string Keyword)
        {
            string searchQuery = "SELECT Description, MeetingRoom FROM EVENT WHERE Description LIKE @Keyword";

            SqlCommand cmdLogin = new SqlCommand();
            cmdLogin.Connection = CapDBConn;
            cmdLogin.Connection.ConnectionString = CapDBConnString;
            cmdLogin.Parameters.AddWithValue("@Keyword", Keyword);
            cmdLogin.CommandText = searchQuery;
            cmdLogin.Connection.Open();
            SqlDataReader tempReader = cmdLogin.ExecuteReader();
            return tempReader;
        }


        public static void AddEventUser(User x)
        {
            using (SqlConnection connection = new SqlConnection(CapDBConnString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO [User] (FirstName, LastName, Email, PhoneNumber, Username) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Username)";

                using (SqlCommand cmdProductRead = new SqlCommand(sqlQuery, connection))
                {
                    ////cmdProductRead.Parameters.AddWithValue("@Username", x.Username);
                    cmdProductRead.Parameters.AddWithValue("@FirstName", x.FirstName);
                    cmdProductRead.Parameters.AddWithValue("@LastName", x.LastName);
                    cmdProductRead.Parameters.AddWithValue("@Email", x.Email);
                    cmdProductRead.Parameters.AddWithValue("@PhoneNumber", x.PhoneNumber);
                    cmdProductRead.Parameters.AddWithValue("@Username", x.Username);
                    
                    

                    cmdProductRead.ExecuteNonQuery();
                }
            }

        }


        public static SqlDataReader GetRequestedEvents()
        {
            String sqlQuery = "Select * from RequestedEvent;";
            SqlCommand cmdEventRead = new SqlCommand();
            cmdEventRead.Connection = CapDBConn;
            cmdEventRead.Connection.ConnectionString = CapDBConnString;
            cmdEventRead.CommandText = sqlQuery;
            cmdEventRead.Connection.Open();

            SqlDataReader x = cmdEventRead.ExecuteReader();
            return x;
        }

    }
}
