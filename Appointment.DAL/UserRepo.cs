using Appointment.DAL;
using Appointment.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DAL
{
    public class UserRepo : IUserRepo
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;

        public UserRepo(string connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            _configuration = configuration;
        }
        public bool IsEmailUnique(string email)
        {
            string connectionString = _connectionString; // Get the connection string from the private field

            // Check if a user with the provided email already exists in the database
            string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                int count = connection.QuerySingleOrDefault<int>(sql, new { Email = email });
                return count == 0; // If count is 0, it means the email is unique
            }
        }

        public List<Users> GetAllUsers()
        {
            List<Users> users = new List<Users>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("GetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var user = new Users()
                    {
                        UserId = Convert.ToInt32(rdr["UserId"]),
                        FirstName = rdr["FirstName"].ToString(),
                        LastName = rdr["LastName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        // Add the remaining columns to populate the Users object
                        Password = rdr["Password"].ToString(), // Assuming Password is returned in plain text
                        RoleName = rdr["RoleName"].ToString(),
                        RoleId = rdr["RoleId"] != DBNull.Value ? Convert.ToInt32(rdr["RoleId"]) : 0, // Provide a default value if DBNull
                        IsActive = rdr["IsActive"] != DBNull.Value ? Convert.ToBoolean(rdr["IsActive"]) : false // Provide a default value if DBNull
                    };
                    users.Add(user);
                }
                return users;
            }
        }



        public Users GetUserById(int userId)
        {
            Users user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetUserById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                // Add the remaining columns to populate the Users object
                                Password = reader["Password"].ToString(), // Assuming Password is returned in plain text
                                //RoleName = reader["RoleName"].ToString(),
                               // RoleId = Convert.ToInt32(reader["RoleId"]),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }
            return user;
        }

        public List<Users> GetUsersByRole(string roleName)
        {
            List<Users> users = new List<Users>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetUsersByRole", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Users user = new Users
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                // Add the remaining columns to populate the Users object
                                Password = reader["Password"].ToString(), // Assuming Password is returned in plain text
                                //RoleId = Convert.ToInt32(reader["RoleId"]),
                                //RoleName = reader["RoleName"].ToString(),
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }


        public Users CreateUser(Users user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Password", user.Password); // Assuming user.Password is already hashed
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);

                    // Add the output parameter for UserId
                    SqlParameter userIdParam = new SqlParameter("@UserId", SqlDbType.Int);
                    userIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(userIdParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Get the UserId from the output parameter
                    int userId = Convert.ToInt32(userIdParam.Value);
                    user.UserId = userId; // Set the UserId in the Users object before returning
                }
            }

            return user; // Return the Users object with the UserId populated
        }



        public Users UpdateUser(Users user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);
                    command.Parameters.AddWithValue("@Password", user.Password); // Assuming user.Password is already hashed or null

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // After the update, retrieve the user details with the updated values from the database
            return GetUserById(user.UserId);
        }


        public bool DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("DeleteUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // Return true to indicate successful deletion
            return true;
        }

        public Users GetUserByEmail(string email)
        {
            Users user = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("GetUserByEmail", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users
                            {
                                UserId = Convert.ToInt32(reader["UserId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString(),
                                // Add the remaining columns to populate the Users object
                                Password = reader["Password"].ToString(), // Assuming Password is returned in plain text
                                IsActive = Convert.ToBoolean(reader["IsActive"])
                            };
                        }
                    }
                }
            }
            return user;
        }
    }
}
