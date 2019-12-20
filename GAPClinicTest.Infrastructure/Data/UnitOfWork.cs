using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Infrastructure.Data
{
 public  class UnitOfWork: IUnitOfWork, IDisposable
    {
        IGenericRepository<Patient> _patientRepository;
        IGenericRepository<Appointment > _appointmentRepository;

        private ClinicContext context;

        public UnitOfWork(ClinicContext clinicContext)
        {
            context = clinicContext;

        }

        public IGenericRepository<Appointment> AppointmentRepository
        {
            get
            {
                if (_appointmentRepository == null)
                {
                    _appointmentRepository = new GenericRepository<Appointment>(context);
                }
                return _appointmentRepository;
            }
        }

        public IGenericRepository<Patient> PatientRepository
        {
            get
            {
                if (_patientRepository == null)
                {
                    _patientRepository = new GenericRepository<Patient>(context);
                }
                return _patientRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
