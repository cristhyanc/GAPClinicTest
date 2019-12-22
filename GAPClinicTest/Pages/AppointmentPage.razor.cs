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
    public class AppointmentPageBase : ComponentBase, IDisposable
    {
        public string SelectedPatientID { get; set; }

        public string[] EnumValues = Enum.GetNames(typeof(AppointmentType));
        public AppointmentType SelectedAppointmentType  { get; set; } 
        protected List<Patient> Patients { get; set; } = new List<Patient>();
        protected Appointment Appointment { get; private set; } = new Appointment();
        [Inject] AppointmentService _appointmentService { get; set; }
        [Inject] PatientService _patientService { get; set; }
        [Inject] NavigationManager _navigationManager { get; set; }

        public EjsDialog ejsDialog { get; set; }

        public string ServerError { get; set; }

        [Parameter]
        public Guid Id { get; set; }
        
      
        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.Appointment.AppointmentDate = DateTime.Now;
                var patientTask = _patientService.GetPatients();
                var appointmentTask= LoadAppointmentInfo();

                await Task.WhenAll(patientTask, appointmentTask);

                if (patientTask.Result?.Count() > 0)
                {
                    this.Patients = patientTask.Result.ToList();
                }
                this.Patients.Insert(0, new Patient { FirstName = " ", LastName = " ", PatientID = Guid.Empty });
               await this.ejsDialog.Hide();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        private async Task LoadAppointmentInfo()
        {
            try
            {
               
                if (this.Id != Guid.Empty)
                {
                    var result = await _appointmentService.GetAppointment(this.Id);
                    if (result != null)
                    {
                        this.Appointment = result;
                        this.SelectedPatientID = this.Appointment.PatientID.ToString();
                        this.SelectedAppointmentType = this.Appointment.AppointmentType;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        protected async Task GoBack()
        {
            _navigationManager.NavigateTo("appointmentsPage/");
        }

        protected async Task SaveAppointment()
        {
            try
            {
                this.Appointment.AppointmentType = this.SelectedAppointmentType;
                this.Appointment.PatientID = Guid.Parse(this.SelectedPatientID);
                var result = await this._appointmentService.SaveAppointment(this.Appointment);
                _navigationManager.NavigateTo("appointmentsPage/");

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

        protected async Task DeleteData()
        {
            try
            {
                await this._appointmentService.CancelAppointment (this.Appointment.AppointmentID );
                _navigationManager.NavigateTo("appointmentsPage/");
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

      
      
        public void OnOverlayclick(object arg)
        {
            this.ejsDialog.Hide();
        }

        public void Dispose()
        {
            
        }
    }
}
