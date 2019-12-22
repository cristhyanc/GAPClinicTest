using GAPClinicTest.Core.Domain.Entities;
using GAPClinicTest.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Core.UseCases
{
   public class PatientUseCase: IPatientUseCase
    {
        IUnitOfWork uow;
        public PatientUseCase(IUnitOfWork work)
        {
            uow = work;
        }

        public void Delete(Guid patientID)
        {
            throw new NotImplementedException();
        }

        public Patient GetPatient(Guid patientID)
        {
            return uow.PatientRepository.GetByID(patientID);
        }

        public IEnumerable<Patient> GetPatients()
        {
            return uow.PatientRepository.Get();
        }

        public Patient Save(Patient patient)
        {
            patient.Save(this.uow);
            uow.Save();
            return patient;
        }

      
    }
}
