using GAPClinicTest.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Core.Interfaces
{
  public  interface IAppointmentUserCase
    {
        IEnumerable<Appointment> GetAppointments();
        Appointment GetAppointment(Guid appointmentId);
        Appointment SaveAppointment(Appointment appointment);
        void DeleteAppointment(Guid appointmentID);
    }
}
