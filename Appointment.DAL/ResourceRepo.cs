using Appointment.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Appointment.DAL;

namespace DAL
{
    public class ResourceRepo : IResourceRepo
    {
        private readonly string _connectionString;

        public ResourceRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Resources> GetAllResources()
        {
            List<Resources> resources = new List<Resources>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                resources = connection.Query<Resources>("SELECT * FROM Resources").ToList();
            }
            return resources;
        }

        public Resources GetResourceById(int resourceId)
        {
            Resources resource = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                resource = connection.QueryFirstOrDefault<Resources>("SELECT * FROM Resources WHERE ResourceId = @ResourceId", new { ResourceId = resourceId });
            }
            return resource;
        }

        public Resources CreateResource(Resources resource)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("CreateResource", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ResourceName", resource.ResourceName);
                    command.Parameters.AddWithValue("@Description", resource.Description);
                    command.Parameters.AddWithValue("@Availability", resource.Availability);

                    // Add the output parameter for ResourceId
                    SqlParameter resourceIdParam = new SqlParameter("@ResourceId", SqlDbType.Int);
                    resourceIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(resourceIdParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Get the ResourceId from the output parameter
                    int resourceId = Convert.ToInt32(resourceIdParam.Value);
                    resource.ResourceId = resourceId; // Set the ResourceId in the Resources object before returning
                }
            }

            return resource; // Return the Resources object with the ResourceId populated
        }


        public Resources UpdateResource(Resources resource)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateResource", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ResourceId", resource.ResourceId);
                    command.Parameters.AddWithValue("@ResourceName", resource.ResourceName);
                    command.Parameters.AddWithValue("@Description", resource.Description);
                    command.Parameters.AddWithValue("@Availability", resource.Availability);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // After the update, retrieve the resource details with the updated values from the database
            return GetResourceById(resource.ResourceId);
        }


        public bool DeleteResource(int resourceId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute("DELETE FROM Resources WHERE ResourceId = @ResourceId", new { ResourceId = resourceId });
            }

            // Return true to indicate successful deletion
            return true;
        }

    }
}
