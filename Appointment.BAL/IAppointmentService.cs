using Appointment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.BAL
{
    public interface IAppointmentService
    {
        List<Appointments> GetAllAppointments(string? searchText);
        Appointments GetAppointmentById(int appointmentId);
        List<Appointments> GetAppointmentsByUserId(int userId);
        List<Appointments> GetAppointmentsByResourceId(int resourceId);
        Appointments CreateAppointment(Appointments appointment);
        Appointments UpdateAppointment(Appointments appointment);
        bool DeleteAppointment(int appointmentId);
        Task<IEnumerable<AcquantainceDetails>> GetResourceDetailsByType(int type);
        Task<IEnumerable<AcquantainceDetails>> GetAvailableAcquaintances(DateTime selectedDate, int professionType);

    }
}
