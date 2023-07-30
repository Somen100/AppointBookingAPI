using Appointment.DAL;
using Appointment.Models;

namespace Appointment.BAL
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public Users CreateUser(Users user)
        {
           return _userRepo.CreateUser(user);
        }

        public bool DeleteUser(int userId)
        {
          return  _userRepo.DeleteUser(userId);
        }

        public List<Users> GetAllUsers()
        {
            return _userRepo.GetAllUsers();
        }

        public Users GetUserById(int userId)
        {
            return _userRepo.GetUserById(userId);
        }

        public List<Users> GetUsersByRole(string roleName)
        {
            return _userRepo.GetUsersByRole(roleName);
        }

        public Users UpdateUser(Users user)
        {
            return _userRepo.UpdateUser(user);
        }

        public bool IsEmailUnique(string email)
        {


            return _userRepo.IsEmailUnique(email);
        }

        public Users GetUserByEmail(string email)
        {
            return _userRepo.GetUserByEmail(email);
        }
    }
}
