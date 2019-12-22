using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace GAPClinicTest.Core.UseCases
{
    public class AppointmentUserCase : IAppointmentUserCase
    {

        IUnitOfWork uow;
        public AppointmentUserCase(IUnitOfWork work)
        {
            uow = work;
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            var appointments= uow.AppointmentRepository .Get();
            foreach (Appointment item in appointments)
            {
                item.Patient = uow.PatientRepository.GetByID(item.PatientID);
            }

            return appointments;
        }

        public Appointment GetAppointment(Guid appointmentID)
        {
            var appointment = uow.AppointmentRepository.GetByID(appointmentID);
            if(appointment!=null)
            {
                appointment.Patient = uow.PatientRepository.GetByID(appointment.PatientID);
            }
            return appointment;
        }

        public Appointment SaveAppointment(Appointment appointment)
        {
            appointment.Save(this.uow);
            uow.Save();
            return appointment;
        }

        public void  DeleteAppointment(Guid appointmentID)
        {
            var appointment = uow.AppointmentRepository.GetByID(appointmentID);
            if(appointment==null)
            {
                throw new ValidationException("Appointment does not exist");
            }
            appointment.Delete(this.uow);
            uow.Save();
        }
    }
}
