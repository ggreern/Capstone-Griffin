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

    }
}
