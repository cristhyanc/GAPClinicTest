using GAPClinicTest.DTO;
using GAPClinicTest.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.Pages
{
    public class PatientPageBase : ComponentBase, IDisposable
    {
        [Inject] PatientService _patientService { get; set; }

        protected List<Patient> Patients { get; private set; } = new List<Patient>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadPatientsInfo();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        private async Task LoadPatientsInfo()
        {
            try
            {

                var result = await _patientService.GetPatients();
                if(result?.Count()>0)
                {
                    this.Patients = new List<Patient>(result);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
