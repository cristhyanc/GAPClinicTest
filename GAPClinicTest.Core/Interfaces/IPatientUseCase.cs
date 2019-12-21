using GAPClinicTest.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GAPClinicTest.Core.Interfaces
{
  public  interface IPatientUseCase
    {
        IEnumerable<Patient> GetPatients();
        bool SavePatient(Patient patient);
    }
}
