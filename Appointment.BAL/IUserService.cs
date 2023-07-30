using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.BAL
{
    public interface IUserService
    {
        bool IsEmailUnique(string email);
        List<Users> GetAllUsers();

        // Get a user by their unique identifier (ID)
        Users GetUserById(int userId);
        Users GetUserByEmail(string email);

        // Create a new user
        Users CreateUser(Users user);

        // Update an existing user
        Users UpdateUser(Users user);

        // Delete a user by their unique identifier (ID)
        bool DeleteUser(int userId);
        List<Users> GetUsersByRole(string roleName);
        
    }
}
