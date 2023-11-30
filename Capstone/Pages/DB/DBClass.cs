using Capstone.Pages.DB;
using Capstone.Pages.Data_Classes;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Capstone.Pages.DB
{
    public class DBClass
    {
        public static SqlConnection CapDBConn = new SqlConnection();
        //AWS Connection String
        //private static readonly string CapDBConnString =
            //@"Server=capstone.chp6q2y6rmvw.us-east-2.rds.amazonaws.com;Database=Capstone;User Id=admin;Password=Capstone12#;Trusted_Connection = False;TrustServerCertificate=true;";

       // private static readonly string AuthConnString =
           // @"Server=capstone.chp6q2y6rmvw.us-east-2.rds.amazonaws.com;Database=AUTH;User Id=admin;Password=Capstone12#;Trusted_Connection=False;TrustServerCertificate=True";

        /*private static readonly string CapDBConnString
            = @"capstone.chp6q2y6rmvw.us-east-2.rds.amazonaws.com;
                    Database=Capstone;uid=admin;password=Capstone12#";
        private static readonly string AuthConnString
           = @"capstone.chp6q2y6rmvw.us-east-2.rds.amazonaws.com;
                    Database=AUTH;uid=admin;password=Capstone12#";*/

        //Local Host Conn
        public static readonly String CapDBConnString = "Server = Localhost;Database = Cap;Trusted_Connection = True;TrustServerCertificate=true;";
        private static readonly String? AuthConnString = "Server=Localhost;Database=AUTH;Trusted_Connection=True;TrustServerCertificate=True";


        public static void InsertEvent(Event eventModel)
        {
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO Event (Name, Description, Address, StartDate, EndDate, RegistrationCost, OrganizerID) 
                                VALUES (@Name, @Description, @Address, @StartDate, @EndDate, @RegistrationCost, @OrganizerID)";

            command.Parameters.AddWithValue("@Name", eventModel.Name);
            command.Parameters.AddWithValue("@Description", eventModel.Description);
            command.Parameters.AddWithValue("@Address", eventModel.Address);
            command.Parameters.AddWithValue("@StartDate", eventModel.StartDate);
            command.Parameters.AddWithValue("@EndDate", eventModel.EndDate);
            command.Parameters.AddWithValue("@RegistrationCost", eventModel.RegistrationCost);
            command.Parameters.AddWithValue("@EventType", eventModel.EventType);
            command.Parameters.AddWithValue("@EstimatedAttendance", eventModel.EstimatedAttendance);
            command.Parameters.AddWithValue("@OrganizerID", eventModel.OrganizerID);


            connection.Open();
            command.ExecuteNonQuery();

            connection.Close();
        }




        public static void InsertRequestedEvent(Event eventModel)
        {
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();

            command.CommandText = @"INSERT INTO RequestedEvent (Name, Description, Address, StartDate, EndDate, RegistrationCost, EventType, EstimatedAttendance, OrganizerID) 
                                VALUES (@Name, @Description, @Address, @StartDate, @EndDate, @RegistrationCost, @EventType, @EstimatedAttendance, @OrganizerID)";

            command.Parameters.AddWithValue("@Name", eventModel.Name);
            command.Parameters.AddWithValue("@Description", eventModel.Description);
            command.Parameters.AddWithValue("@Address", eventModel.Address);
            command.Parameters.AddWithValue("@StartDate", eventModel.StartDate);
            command.Parameters.AddWithValue("@EndDate", eventModel.EndDate);
            command.Parameters.AddWithValue("@RegistrationCost", eventModel.RegistrationCost);
            command.Parameters.AddWithValue("@EventType", eventModel.EventType);
            command.Parameters.AddWithValue("@EstimatedAttendance", eventModel.EstimatedAttendance);
            command.Parameters.AddWithValue("@OrganizerID", eventModel.OrganizerID);

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

            using (SqlConnection connection = new SqlConnection(AuthConnString))
            {
                using (SqlCommand cmdLogin = new SqlCommand(loginQuery, connection))
                {
                    cmdLogin.Parameters.AddWithValue("@Username", Username);
                    cmdLogin.Parameters.AddWithValue("@Password", PasswordHash.HashPassword(Password));

                    connection.Open();

                    cmdLogin.ExecuteNonQuery();
                }
            }
        }


        public static bool HashedParameterLogin(string Username, string Password)
        {
            string loginQuery = "SELECT * from HashedCredentials Where Username = @Username";

            using (SqlConnection connection = new SqlConnection(AuthConnString))
            {
                using (SqlCommand cmdLogin = new SqlCommand(loginQuery, connection))
                {
                    cmdLogin.Parameters.AddWithValue("@Username", Username);

                    connection.Open();

                    using (SqlDataReader hashReader = cmdLogin.ExecuteReader())
                    {
                        if (hashReader.Read())
                        {
                            string correctHash = hashReader["Password"].ToString();

                            if (PasswordHash.ValidatePassword(Password, correctHash))
                            {
                                return true;
                            }
                        }
                    }
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

                string sqlQuery = "INSERT INTO [User] (FirstName, LastName, Email, PhoneNumber, Username, UserType) VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Username, @UserType)";

                using (SqlCommand cmdProductRead = new SqlCommand(sqlQuery, connection))
                {
                    cmdProductRead.Parameters.AddWithValue("@FirstName", x.FirstName);
                    cmdProductRead.Parameters.AddWithValue("@LastName", x.LastName);
                    cmdProductRead.Parameters.AddWithValue("@Email", x.Email);
                    cmdProductRead.Parameters.AddWithValue("@PhoneNumber", x.PhoneNumber);
                    cmdProductRead.Parameters.AddWithValue("@Username", x.Username);
                    cmdProductRead.Parameters.AddWithValue("@UserType", "Attendee");

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


        public static Event GetRequestedEventDetails(string EventName)
        {
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM RequestedEvent WHERE Name = @EventName";
            command.Parameters.AddWithValue("@EventName", EventName);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            Event eventDetails = null;
            if (reader.Read())
            {
                eventDetails = new Event
                {

                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),
                    Address = reader.GetString(reader.GetOrdinal("Address")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")).ToString(),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")).ToString(),
                    RegistrationCost = (int)reader.GetDecimal(reader.GetOrdinal("RegistrationCost")),
                    EventType = reader.GetString(reader.GetOrdinal("EventType")),
                    EstimatedAttendance = reader.GetInt32(reader.GetOrdinal("EstimatedAttendance")),
                    OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID"))
                };
            }
            connection.Close();
            return eventDetails;
        }

        public static List<SelectListItem> GetAllEvents()
        {
            List<SelectListItem> events = new List<SelectListItem>();
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EventID, Name FROM Event";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new SelectListItem
                        {
                            Value = reader["EventID"].ToString(),
                            Text = reader["Name"].ToString()
                        });
                    }
                }
            }
            return events;
        }

        public static void AddSubEvent(SubEvent subEvent)
        {
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO SubEvent (Name, Description, SubEventType, EstimatedAttendance, EventID, HostID) VALUES (@Name, @Description, @SubEventType, @EstimatedAttendance, @EventID, @HostID)";
                command.Parameters.AddWithValue("@Name", subEvent.Name);
                command.Parameters.AddWithValue("@Description", subEvent.Description);
                command.Parameters.AddWithValue("@SubEventType", subEvent.SubEventType);
                command.Parameters.AddWithValue("@EstimatedAttendance", subEvent.EstimatedAttendance);
                command.Parameters.AddWithValue("@EventID", subEvent.EventID);
                command.Parameters.AddWithValue("@HostID", subEvent.HostID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public static SqlDataReader GetEventID (string EventName)
        {
            SqlCommand cmdGetID = new SqlCommand();
            cmdGetID.Connection = CapDBConn;
            cmdGetID.Connection.ConnectionString = CapDBConnString;
            cmdGetID.CommandText = "Select EventID FROM Event Where Name = @EventName";
            cmdGetID.Parameters.AddWithValue("@EventName", EventName);
            cmdGetID.Connection.Open();
            SqlDataReader tempReader = cmdGetID.ExecuteReader();
            return tempReader;
        }

        public static int GetUserIDByName(string userName)
        {
            int userID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(CapDBConnString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT UserID FROM [User] WHERE userName = @UserName", connection))
                    {
                        command.Parameters.AddWithValue("@UserName", userName);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            userID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, or handle as appropriate)
                Console.WriteLine($"Error in GetUserIDByName: {ex.Message}");
                // You might want to log or throw the exception, or handle it in a way that's suitable for your application.
            }

            return userID;
        }



        public static string GetUserTypeFromSession(HttpContext httpContext)
        {
            // Check if the username is in the session
            string username = httpContext.Session.GetString("username");

            if (username != null)
            {
                // Use your existing method to get the UserType from the database
                string userType = GetUserTypeByName(username);

                return userType;
            }

            return null; // or some default value depending on your requirements
        }

        public static string GetUserTypeByName(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(CapDBConnString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT UserType FROM [User] WHERE Username = @Username", connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, or handle as appropriate)
                Console.WriteLine($"Error in GetUserTypeByName: {ex.Message}");
                // You might want to log or throw the exception, or handle it in a way that's suitable for your application.
            }

            return null; // or some default value depending on your requirements
        }

        public static List<SelectListItem> GetAllEventsList()
        {
            List<SelectListItem> events = new List<SelectListItem>();
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EventID, Name FROM Event";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new SelectListItem
                        {
                            Value = reader["EventID"].ToString(),
                            Text = reader["Name"].ToString()
                        });
                    }
                }
            }
            return events;
        }


        public static List<User> GetUsersForSelectedEvents(List<int> selectedEventIds)
        {
            List<User> users = new List<User>();

            // Check if there are selected event IDs
            if (selectedEventIds != null && selectedEventIds.Count > 0)
            {
                // Use a parameterized query to fetch users for the selected events
                string query = "SELECT * FROM [User] INNER JOIN EventRegistration ON [User].UserID = EventRegistration.UserID WHERE EventRegistration.EventID IN (@EventIds)";

                using (SqlConnection connection = new SqlConnection(CapDBConnString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use a table-valued parameter to pass the list of event IDs
                        var table = new DataTable();
                        table.Columns.Add("EventId", typeof(int));
                        foreach (var eventId in selectedEventIds)
                        {
                            table.Rows.Add(eventId);
                        }

                        SqlParameter param = command.Parameters.AddWithValue("@EventIds", table);
                        param.SqlDbType = System.Data.SqlDbType.Structured;
                        param.TypeName = "dbo.IntList"; // Assuming you have a user-defined table type named IntList

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    // Map the user properties from the reader
                                    UserID = reader.GetInt32(reader.GetOrdinal("UserID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    UserType = reader.GetString(reader.GetOrdinal("UserType"))
                                    
                                };

                                users.Add(user);
                            }
                        }
                    }
                }
            }

            return users;
        }



        public static List<Event> GetEventsList()
        {
            List<Event> events = new List<Event>();

            using (SqlConnection connection = new SqlConnection(CapDBConnString))
            {
                connection.Open();

                string sqlQuery = "SELECT EventID, Name, Description FROM Event";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event eventItem = new Event
                            {
                                EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.GetString(reader.GetOrdinal("Description"))
                            };

                            events.Add(eventItem);
                        }
                    }
                }
            }

            return events;
        }


        public static void AddEventRegistration(int userId, int eventId)
        {
            using (SqlConnection connection = new SqlConnection(CapDBConnString))
            {
                connection.Open();

                string sqlQuery = "INSERT INTO EventRegistration (UserID, EventID) VALUES (@UserID, @EventID)";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@EventID", eventId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<SelectListItem> GetAllSubEvents()
        {
            List<SelectListItem> subEvents = new List<SelectListItem>();
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT SubEventID, Name FROM SubEvent";

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subEvents.Add(new SelectListItem
                        {
                            Value = reader["SubEventID"].ToString(),
                            Text = reader["Name"].ToString()
                        });
                    }
                }
            }
            return subEvents;
        }


        public static List<Room> GetSuggestedRooms(int estimatedAttendance)
        {
            List<Room> rooms = new List<Room>();
            var connection = new SqlConnection(CapDBConnString);
            var command = connection.CreateCommand();
            command.CommandText = "SELECT RoomID, Name, Capacity FROM Room WHERE Capacity >= @EstimatedAttendance";
            command.Parameters.AddWithValue("@EstimatedAttendance", estimatedAttendance);

            connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                rooms.Add(new Room
                {
                    RoomID = reader.GetInt32(reader.GetOrdinal("RoomID")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Capacity = reader.GetInt32(reader.GetOrdinal("Capacity"))
                });
            }
            connection.Close();
            return rooms;
        }

        public static int GetEstimatedAttendanceForSubEvent(int subEventId)
        {
            int estimatedAttendance = 0;
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EstimatedAttendance FROM SubEvent WHERE SubEventID = @SubEventID";
                command.Parameters.AddWithValue("@SubEventID", subEventId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estimatedAttendance = reader.GetInt32(reader.GetOrdinal("EstimatedAttendance"));
                    }
                }
            }
            return estimatedAttendance;
        }

        public static int GetEstimatedAttendanceForEvent(int eventId)
        {
            int estimatedAttendance = 0;
            using (var connection = new SqlConnection(CapDBConnString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT EstimatedAttendance FROM Event WHERE EventID = @EventID";
                command.Parameters.AddWithValue("@EventID", eventId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        estimatedAttendance = reader.GetInt32(reader.GetOrdinal("EstimatedAttendance"));
                    }
                }
            }
            return estimatedAttendance;
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(CapDBConnString))
            {
                connection.Open();

                string sqlQuery = "SELECT FirstName, LastName FROM [User]";

                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {

                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName"))
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }


        public static List<Event> GetEventsForAttendee(int userId)
        {
            List<Event> events = new List<Event>();

            try
            {
                using (SqlConnection connection = new SqlConnection(CapDBConnString))
                {
                    connection.Open();

                    // Fetch events for the attendee based on EventRegistration
                    string query = @"
                SELECT Event.* 
                FROM Event
                INNER JOIN EventRegistration ON Event.EventID = EventRegistration.EventID
                WHERE EventRegistration.UserID = @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Event ev = new Event
                                {
                                    EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")).ToString(),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")).ToString(),
                                    RegistrationCost = (int)reader.GetDecimal(reader.GetOrdinal("RegistrationCost")),
                                    EventType = reader.GetString(reader.GetOrdinal("EventType")),
                                    EstimatedAttendance = reader.GetInt32(reader.GetOrdinal("EstimatedAttendance")),
                                    OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID"))
                                };

                                events.Add(ev);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, or handle as appropriate)
                Console.WriteLine($"Error in GetEventsForAttendee: {ex.Message}");
            }

            return events;
        }

        public static List<Event> GetEventsForOrganizer(int userId)
        {
            List<Event> events = new List<Event>();

            try
            {
                using (SqlConnection connection = new SqlConnection(CapDBConnString))
                {
                    connection.Open();

                    // Fetch events for the organizer based on OrganizerID in Event table
                    string query = "SELECT * FROM Event WHERE OrganizerID = @OrganizerID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrganizerID", userId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Event ev = new Event
                                {
                                    EventID = reader.GetInt32(reader.GetOrdinal("EventID")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")).ToString(),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")).ToString(),
                                    RegistrationCost = (int)reader.GetDecimal(reader.GetOrdinal("RegistrationCost")),
                                    EventType = reader.GetString(reader.GetOrdinal("EventType")),
                                    EstimatedAttendance = reader.GetInt32(reader.GetOrdinal("EstimatedAttendance")),
                                    OrganizerID = reader.GetInt32(reader.GetOrdinal("OrganizerID"))
                                };

                                events.Add(ev);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, throw, or handle as appropriate)
                Console.WriteLine($"Error in GetEventsForOrganizer: {ex.Message}");
            }

            return events;
        }






    }
}
