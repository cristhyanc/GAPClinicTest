using GAPClinicTest.DTO;
using GAPClinicTest.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.EJ2.Blazor.Grids;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GAPClinicTest.Pages
{
    public class AppointmentsPageBase : ComponentBase, IDisposable
    {
        protected List<Appointment> Appointments { get; private set; } = new List<Appointment>();

        protected Appointment SelectedAppointment { get; set; } = new Appointment();

        [Inject] AppointmentService _appointmentService { get; set; }   

      

        public string IsBusyString
        {
            get
            {
                if (IsBusy)
                {
                    return "visible";
                }
                return "hidden";

            }
        }

        bool _isBusy;
        protected bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                this.StateHasChanged();
            }
        }

    

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await InitInformation();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }

        private async Task InitInformation()
        {
            try
            {           
               
               await LoadInfoGrid();               
            }
            catch (Exception ex)
            {

                Console.WriteLine("InitInformation " + ex.Message);
            }
            finally
            {
              //  this.IsBusy = false;
            }
        }


        private async Task LoadInfoGrid()
        {
            try
            {
                this.Appointments = new List<Appointment>();
               var appointmentTask = await _appointmentService.GetAppointments();
                if (appointmentTask?.Count() > 0)
                {
                    this.Appointments = appointmentTask.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadInfoGrid " + ex.Message);
            }
        }

      

    
        //public void RowSelectHandler(RowSelectEventArgs<Appointment > args)
        //{
        //    try
        //    {
        //        this.SelectedAppointment = args.Data;
        //        this.ddlVal = this.SelectedAppointment.AppointmentType;
        //        Console.WriteLine(this.SelectedAppointment.PatientID.ToString());
        //        if (this.SelectedAppointment.PatientID != null)
        //        {
        //            this.SelectedPatientID = this.SelectedAppointment.PatientID.ToString();
        //        }
        //        //StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
           
        //}

        public void Dispose()
        {
            
        }
    }
}
