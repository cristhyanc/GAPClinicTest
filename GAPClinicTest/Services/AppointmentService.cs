using GAPClinicTest.DTO;
using GAPClinicTest.Rest;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace GAPClinicTest.Services
{
    public class AppointmentService
    {
        RestClient _restService;
        string _serverUrl;
        public AppointmentService(string token, string serverUrl)
        {
            _restService = new RestClient(token);
            _serverUrl = serverUrl;
        }

        public async Task<IEnumerable<Appointment >> GetAppointments(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _restService.MakeApiCall<IEnumerable<Appointment>>($"{_serverUrl}/Appointment", HttpMethod.Get, null, cancellationToken);
        }

        public async Task<Appointment> GetAppointment(Guid appointmentID,  CancellationToken cancellationToken = default(CancellationToken))
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = appointmentID.ToString();
            return await _restService.MakeApiCall<Appointment>($"{_serverUrl}/Appointment/GetById", HttpMethod.Get, queryString, cancellationToken);
        }

        public async Task<bool > SaveAppointment(Appointment appointment, CancellationToken cancellationToken = default(CancellationToken))
        {           
             return await _restService.MakeApiCall($"{_serverUrl}/Appointment/", HttpMethod.Post , appointment, cancellationToken);
        }

        public async Task<bool> CancelAppointment(Guid appointmentID, CancellationToken cancellationToken = default(CancellationToken))
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryString["id"] = appointmentID.ToString();
            return await _restService.MakeApiCall($"{_serverUrl}/Appointment/Delete", HttpMethod.Delete, queryString, cancellationToken);
        }
    }
}
