using GAPClinicTest.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Core.Interfaces
{
  public  interface IUnitOfWork: IDisposable 
    {
        IGenericRepository<Patient> PatientRepository { get; }
        IGenericRepository<Appointment> AppointmentRepository { get; }
        void Save();
    }
}
