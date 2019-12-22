using GAPClinicTest.DTO;
using GAPClinicTest.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.EJ2.Blazor.Popups;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.Pages
{
    public class PatientPageBase : ComponentBase, IDisposable
    {
        [Inject] PatientService _patientService { get; set; }

        public Patient Patient { get; set; } = new Patient();
        [Inject] NavigationManager _navigationManager { get; set; }

        public EjsDialog ejsDialog { get; set; }

        public string ServerError { get; set; }
        [Parameter]
        public Guid Id { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {                            
                this.Patient.DOB = DateTime.Now;
                await LoadPatientInfo();
                if (this.ejsDialog != null)
                {
                    await this.ejsDialog.Hide();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace );
            }

        }

        protected override bool ShouldRender()
        {
            if (this.ejsDialog != null)
            {                
                this.ejsDialog.Hide();
            }
            return base.ShouldRender();
        }

        private async Task LoadPatientInfo()
        {
            try
            {

                if (this.Id!=null && this.Id != Guid.Empty)
                {
                    var result = await _patientService.GetPatient(this.Id);
                    if (result != null)
                    {
                        this.Patient = result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task SavePatient()
        {
            try
            {
                var result = await this._patientService.Save(this.Patient);
                _navigationManager.NavigateTo("patientspage/");

            }
            catch (ValidationException ex)
            {
                ServerError = ex.Message;
                await ejsDialog.Show();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task GoBack()
        {
            _navigationManager.NavigateTo("patientspage/");
        }

        public void OnOverlayclick(object arg)
        {
            this.ejsDialog.Hide();
        }
        public void Dispose()
        {

        }
    }
}
