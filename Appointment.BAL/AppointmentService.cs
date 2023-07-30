using Appointment.DAL;
using Appointment.Models;
using System;
using System.Collections.Generic;

namespace Appointment.BAL
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepo _appointmentRepo;

        public AppointmentService(IAppointmentRepo appointmentRepo)
        {
            _appointmentRepo = appointmentRepo;
        }

        public Appointments CreateAppointment(Appointments appointment)
        {
            // Call the CreateAppointment method of the repository to create the appointment
            return _appointmentRepo.CreateAppointment(appointment);
        }

        public bool DeleteAppointment(int appointmentId)
        {
            // Call the DeleteAppointment method of the repository to delete the appointment
            return _appointmentRepo.DeleteAppointment(appointmentId);
        }

        public List<Appointments> GetAllAppointments(string? searchText)
        {
            // Call the GetAllAppointments method of the repository to get all appointments
            return _appointmentRepo.GetAllAppointments(searchText);
        }

        public Appointments GetAppointmentById(int appointmentId)
        {
            // Call the GetAppointmentById method of the repository to get the appointment by ID
            return _appointmentRepo.GetAppointmentById(appointmentId);
        }

        public List<Appointments> GetAppointmentsByResourceId(int resourceId)
        {
            // Call the GetAppointmentsByResourceId method of the repository to get appointments by resource ID
            return _appointmentRepo.GetAppointmentsByResourceId(resourceId);
        }

        public List<Appointments> GetAppointmentsByUserId(int userId)
        {
            // Call the GetAppointmentsByUserId method of the repository to get appointments by user ID
            return _appointmentRepo.GetAppointmentsByUserId(userId);
        }

        public Task<IEnumerable<AcquantainceDetails>> GetAvailableAcquaintances(DateTime selectedDate, int professionType)
        {
            return _appointmentRepo.GetAvailableAcquaintances(selectedDate, professionType);
        }

        public Task<IEnumerable<AcquantainceDetails>> GetResourceDetailsByType(int type)
        {
            return _appointmentRepo.GetResourceDetailsByType(type);

        }

        public Appointments UpdateAppointment(Appointments appointment)
        {
            // Call the UpdateAppointment method of the repository to update the appointment
            return _appointmentRepo.UpdateAppointment(appointment);
        }
    }
}
