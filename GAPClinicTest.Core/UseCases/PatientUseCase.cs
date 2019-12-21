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


        public IEnumerable<Patient> GetPatients()
        {
            return uow.PatientRepository.Get();
        }

        public bool SavePatient(Patient patient )
        {
            return true;
        }
    }
}
