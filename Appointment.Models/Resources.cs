using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Resources
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; }
        public string Description { get; set; }
        public string Availability { get; set; }
    }

    // Teacher API Model
    public class Teachers
    {
        public int TeacherId { get; set; }
        public int? ResourceId { get; set; }
        public string TeacherName { get; set; }
        public bool? IsActive { get; set; }
    }

    // Doctor API Model
    public class Doctors
    {
        public int DoctorId { get; set; }
        public int? ResourceId { get; set; }
        public string DoctorName { get; set; }
        public bool? IsActive { get; set; }
    }

    // GymTrainer API Model
    public class GymTrainers
    {
        public int GymTrainerId { get; set; }
        public int? ResourceId { get; set; }
        public string GymTrainerName { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AcquantainceDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ResourceId { get; set; }
        public bool? IsActive { get; set; }
        public string? AvailableDate { get; set; }
    }

}
