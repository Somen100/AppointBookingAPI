using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Models
{
    public class Appointments
    {
        public int AppointmentId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? ResourceName { get; set; }
        public int ResourceId { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AcquaintanceName { get; set; }
        public string? ClientName { get; set; }

    }

}
