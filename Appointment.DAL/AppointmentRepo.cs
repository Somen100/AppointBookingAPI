using Appointment.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Appointment.DAL;
using System.Globalization;

namespace DAL
{
    public class AppointmentRepo : IAppointmentRepo
    {
        private readonly string _connectionString;

        public AppointmentRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Appointments> GetAllAppointments(string? searchText)
        {
            List<Appointments> appointments = new List<Appointments>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var parameters = new { searchText }; // Create parameter object

                // Execute the stored procedure using Dapper
                if (string.IsNullOrEmpty(searchText))
                {
                    // If searchText is empty or null, call the first part of the stored procedure
                    appointments = connection.Query<Appointments>("sp_GetAllAppointments", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
                else
                {
                    // If searchText is not empty, call the second part of the stored procedure
                    appointments = connection.Query<Appointments>("sp_GetAllAppointments", parameters, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return appointments;
        }

        public Appointments GetAppointmentById(int appointmentId)
        {
            Appointments appointment = new Appointments();
            var parameters = new DynamicParameters();
            parameters.Add("@Id", appointmentId, DbType.Int64, ParameterDirection.Input);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                appointment = connection.QuerySingleOrDefault<Appointments>("sp_GetAllAppointmentById", parameters, commandType: CommandType.StoredProcedure);
            }

            return appointment;
        }

       

        public List<Appointments> GetAppointmentsByUserId(int userId)
        {
            List<Appointments> appointments = new List<Appointments>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                appointments = connection.Query<Appointments>("SELECT * FROM Appointments WHERE UserId = @UserId", new { UserId = userId }).ToList();
            }
            return appointments;
        }

        public List<Appointments> GetAppointmentsByResourceId(int resourceId)
        {
            List<Appointments> appointments = new List<Appointments>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                appointments = connection.Query<Appointments>("SELECT * FROM Appointments WHERE ResourceId = @ResourceId", new { ResourceId = resourceId }).ToList();
            }
            return appointments;
        }

        public bool DeleteAppointment(int appointmentId)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                // Use the Dapper's Execute method to execute the stored procedure
                dbConnection.Execute("DeleteAppointment", new { Id = appointmentId }, commandType: CommandType.StoredProcedure);
            }
            return true;
        }

        public Appointments CreateAppointment(Appointments appointment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("CreateAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Set the @AppointmentIdNo parameter based on whether it's a new or existing appointment
                    if (appointment.AppointmentId == null || appointment.AppointmentId == 0)
                    {
                        command.Parameters.AddWithValue("@AppointmentIdNo", DBNull.Value); // Set as NULL
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@AppointmentIdNo", appointment.AppointmentId);
                    }

                    // Set other parameters
                    command.Parameters.AddWithValue("@UserId", appointment.UserId);
                    command.Parameters.AddWithValue("@ClientName", appointment.Username);
                    command.Parameters.AddWithValue("@ResourceId", appointment.ResourceId);
                    command.Parameters.AddWithValue("@StartTime", appointment.StartTime);
                    command.Parameters.AddWithValue("@EndTime", appointment.EndTime);
                    command.Parameters.AddWithValue("@IsCancelled", appointment.IsCancelled);
                    command.Parameters.AddWithValue("@CreatedAt", appointment.CreatedAt);
                    command.Parameters.AddWithValue("@AcquaintanceName", appointment.AcquaintanceName);

                    // Add the output parameter for AppointmentId
                    SqlParameter appointmentIdParam = new SqlParameter("@AppointmentId", SqlDbType.Int);
                    appointmentIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(appointmentIdParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Get the AppointmentId from the output parameter
                    if (appointment.AppointmentId == null || appointment.AppointmentId == 0)
                    {
                        // If it's a new appointment, set the AppointmentId in the Appointments object before returning
                        appointment.AppointmentId = Convert.ToInt32(appointmentIdParam.Value);
                    }
                }
            }

            return appointment; // Return the Appointments object with the AppointmentId populated
        }





        public Appointments UpdateAppointment(Appointments appointment)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentId", appointment.AppointmentId);
                    command.Parameters.AddWithValue("@UserId", appointment.UserId);
                    command.Parameters.AddWithValue("@ResourceId", appointment.ResourceId);
                    command.Parameters.AddWithValue("@StartTime", appointment.StartTime);
                    command.Parameters.AddWithValue("@EndTime", appointment.EndTime);
                    command.Parameters.AddWithValue("@IsCancelled", appointment.IsCancelled);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // After the update, retrieve the appointment details with the updated values from the database
            return GetAppointmentById(appointment.AppointmentId);
        }


       
        public async Task<IEnumerable<AcquantainceDetails>> GetResourceDetailsByType(int type)
        {
            using (IDbConnection dbConnection = new SqlConnection(_connectionString))
            {
                dbConnection.Open();
                var result = await dbConnection.QueryAsync<AcquantainceDetails>("GetResourceDetailsById", new { Type = type }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<AcquantainceDetails>> GetAvailableAcquaintances(DateTime selectedDate, int professionType)
        {
            try
            {
                using (IDbConnection dbConnection = new SqlConnection(_connectionString))
                {
                    dbConnection.Open();

                    var parameters = new { SelectedDate = selectedDate, ProfessionType = professionType };
                    var result = await dbConnection.QueryAsync<AcquantainceDetails>("GetAvailableAcquaintances", parameters, commandType: CommandType.StoredProcedure);

                    return result;
                }
            }
            catch (Exception)
            {
                return null; // Return null in case of any error
            }
        }


    }
}
